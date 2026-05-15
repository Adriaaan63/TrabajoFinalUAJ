"""Metrics Worker para la pipeline de telemetria FPS.

Flujo:
  Redis(sessions_to_process) -> MongoDB(raw_events) -> PostgreSQL(metricas)

Escribe el contrato que consume la Query API:
  - player_stats
  - session_stats
  - death_events
  - position_events
  - item_events
"""
from __future__ import annotations

import os
import time
from datetime import datetime, timezone
from typing import Any, Callable

import redis
from bson import ObjectId
from bson.errors import InvalidId
from pymongo import MongoClient
from pymongo.collection import Collection
from sqlalchemy import create_engine, text
from sqlalchemy.engine import Engine
from sqlalchemy.exc import DBAPIError, OperationalError

# ---------------------------------------------------------------------------
# Configuracion
# ---------------------------------------------------------------------------

MONGO_URL = os.getenv("MONGO_URL")
POSTGRES_URL = os.getenv("POSTGRES_URL")
MONGO_DB_NAME = os.getenv("MONGO_DB_NAME", "telemetry_db")
MONGO_COLLECTION = os.getenv("MONGO_COLLECTION", "raw_events")
REDIS_HOST = os.getenv("REDIS_HOST", "redis-server")
REDIS_PORT = int(os.getenv("REDIS_PORT", "6379"))
REDIS_DB = int(os.getenv("REDIS_DB", "0"))
REDIS_QUEUE_KEY = os.getenv("REDIS_QUEUE_KEY", "sessions_to_process")

EVENT_KILL = os.getenv("EVENT_KILL", "Player_Kill")
EVENT_DEATH = os.getenv("EVENT_DEATH", "Player_Death")
EVENT_SHOT = os.getenv("EVENT_SHOT", "Player_Shot")
EVENT_HIT = os.getenv("EVENT_HIT", "Player_Hit")
EVENT_SPAWN = os.getenv("EVENT_SPAWN", "Player_Spawn")
EVENT_SESSION_START = os.getenv("EVENT_SESSION_START", "Session_Start")
EVENT_SESSION_END = os.getenv("EVENT_SESSION_END", "Session_End")
EVENT_POSITION = os.getenv("EVENT_POSITION", "Player_Position_Heartbeat")
EVENT_ITEM = os.getenv("EVENT_ITEM", "Item_Picked")

if not MONGO_URL:
    raise RuntimeError("Falta MONGO_URL en el entorno")
if not POSTGRES_URL:
    raise RuntimeError("Falta POSTGRES_URL en el entorno")

mongo_client = MongoClient(MONGO_URL)
events_col: Collection = mongo_client[MONGO_DB_NAME][MONGO_COLLECTION]

pg_engine: Engine = create_engine(
    POSTGRES_URL,
    pool_pre_ping=True,
    pool_recycle=1800,
    future=True,
)

redis_client = redis.Redis(
    host=REDIS_HOST,
    port=REDIS_PORT,
    db=REDIS_DB,
    decode_responses=True,
)

# ---------------------------------------------------------------------------
# DDL PostgreSQL
# ---------------------------------------------------------------------------

SCHEMA_SQL = """
CREATE TABLE IF NOT EXISTS player_stats (
    player_id              VARCHAR(64)   PRIMARY KEY,
    total_kills            INTEGER       NOT NULL DEFAULT 0,
    total_deaths           INTEGER       NOT NULL DEFAULT 0,
    kd_ratio               NUMERIC(10,4) NOT NULL DEFAULT 0,
    avg_accuracy           NUMERIC(5,4)  NOT NULL DEFAULT 0,
    avg_ttl_seconds        NUMERIC(8,2)  NOT NULL DEFAULT 0,
    total_sessions         INTEGER       NOT NULL DEFAULT 0,
    total_playtime_seconds INTEGER       NOT NULL DEFAULT 0,
    items_picked           INTEGER       NOT NULL DEFAULT 0,
    last_updated           TIMESTAMPTZ   NOT NULL DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS session_stats (
    session_id        VARCHAR(64)   PRIMARY KEY,
    player_id         VARCHAR(64)   NOT NULL REFERENCES player_stats(player_id) ON DELETE CASCADE,
    started_at        TIMESTAMPTZ   NOT NULL,
    ended_at          TIMESTAMPTZ,
    duration_seconds  INTEGER       NOT NULL DEFAULT 0,
    kills             INTEGER       NOT NULL DEFAULT 0,
    deaths            INTEGER       NOT NULL DEFAULT 0,
    kd_ratio          NUMERIC(10,4) NOT NULL DEFAULT 0,
    accuracy          NUMERIC(5,4)  NOT NULL DEFAULT 0,
    avg_ttl_seconds   NUMERIC(8,2)  NOT NULL DEFAULT 0,
    shots_fired       INTEGER       NOT NULL DEFAULT 0,
    shots_hit         INTEGER       NOT NULL DEFAULT 0,
    items_picked      INTEGER       NOT NULL DEFAULT 0
);

CREATE INDEX IF NOT EXISTS idx_session_player_time
    ON session_stats (player_id, started_at DESC);

CREATE TABLE IF NOT EXISTS death_events (
    id            BIGSERIAL        PRIMARY KEY,
    session_id    VARCHAR(64)      NOT NULL,
    player_id     VARCHAR(64),
    pos_x         DOUBLE PRECISION NOT NULL,
    pos_z         DOUBLE PRECISION NOT NULL,
    floor_id      INTEGER,
    killer_id     VARCHAR(64),
    is_ai         BOOLEAN          NOT NULL DEFAULT FALSE,
    ttl_seconds   NUMERIC(8,2),
    occurred_at   TIMESTAMPTZ      NOT NULL
);

CREATE INDEX IF NOT EXISTS idx_death_floor   ON death_events (floor_id);
CREATE INDEX IF NOT EXISTS idx_death_session ON death_events (session_id);
CREATE INDEX IF NOT EXISTS idx_death_player  ON death_events (player_id);

CREATE TABLE IF NOT EXISTS position_events (
    id           BIGSERIAL        PRIMARY KEY,
    session_id   VARCHAR(64)      NOT NULL,
    player_id    VARCHAR(64)      NOT NULL,
    pos_x        DOUBLE PRECISION NOT NULL,
    pos_z        DOUBLE PRECISION NOT NULL,
    floor_id     INTEGER,
    recorded_at  TIMESTAMPTZ      NOT NULL
);

CREATE INDEX IF NOT EXISTS idx_position_floor  ON position_events (floor_id);
CREATE INDEX IF NOT EXISTS idx_position_player ON position_events (player_id);

CREATE TABLE IF NOT EXISTS item_events (
    id           BIGSERIAL    PRIMARY KEY,
    session_id   VARCHAR(64)  NOT NULL,
    player_id    VARCHAR(64)  NOT NULL,
    item_type    VARCHAR(32)  NOT NULL,
    occurred_at  TIMESTAMPTZ  NOT NULL
);

CREATE INDEX IF NOT EXISTS idx_item_player ON item_events (player_id);
CREATE INDEX IF NOT EXISTS idx_item_type   ON item_events (item_type);
"""

MIGRATIONS_SQL = """
ALTER TABLE player_stats ADD COLUMN IF NOT EXISTS kd_ratio NUMERIC(10,4) NOT NULL DEFAULT 0;
ALTER TABLE player_stats ADD COLUMN IF NOT EXISTS avg_ttl_seconds NUMERIC(8,2) NOT NULL DEFAULT 0;
ALTER TABLE player_stats ADD COLUMN IF NOT EXISTS total_sessions INTEGER NOT NULL DEFAULT 0;
ALTER TABLE player_stats ADD COLUMN IF NOT EXISTS total_playtime_seconds INTEGER NOT NULL DEFAULT 0;
ALTER TABLE player_stats ADD COLUMN IF NOT EXISTS items_picked INTEGER NOT NULL DEFAULT 0;
ALTER TABLE player_stats ADD COLUMN IF NOT EXISTS last_updated TIMESTAMPTZ NOT NULL DEFAULT NOW();
"""

# ---------------------------------------------------------------------------
# Helpers
# ---------------------------------------------------------------------------

def retry_with_backoff(
    fn: Callable[[], Any],
    *,
    max_attempts: int = 8,
    base_delay: float = 2.0,
    max_delay: float = 30.0,
    label: str = "",
) -> Any:
    attempt = 0
    delay = base_delay
    while True:
        attempt += 1
        try:
            return fn()
        except (OperationalError, DBAPIError) as exc:
            if attempt >= max_attempts:
                print(f"[worker] {label} fallo tras {attempt} intentos: {exc}")
                raise
            detail = getattr(exc, "orig", exc)
            print(
                f"[worker] {label} intento {attempt}/{max_attempts} fallido "
                f"({type(exc).__name__}: {detail}). Reintento en {delay:.1f}s..."
            )
            time.sleep(delay)
            delay = min(delay * 2, max_delay)


def flatten_events(raw: Any) -> list[dict[str, Any]]:
    """Acepta eventos como dict, lista de dicts o listas anidadas."""
    flat: list[dict[str, Any]] = []
    if isinstance(raw, dict):
        flat.append(raw)
    elif isinstance(raw, list):
        for item in raw:
            flat.extend(flatten_events(item))
    return flat


def event_type(event: dict[str, Any]) -> str:
    return str(event.get("event_type") or event.get("type") or "")


def parse_time(value: Any) -> datetime | None:
    if value is None:
        return None
    if isinstance(value, datetime):
        return value if value.tzinfo else value.replace(tzinfo=timezone.utc)
    if isinstance(value, (int, float)):
        seconds = value / 1000.0 if value > 1_000_000_000_000 else float(value)
        return datetime.fromtimestamp(seconds, tz=timezone.utc)
    if isinstance(value, str):
        raw = value.strip()
        if not raw:
            return None
        if raw.isdigit():
            return parse_time(int(raw))
        try:
            return datetime.fromisoformat(raw.replace("Z", "+00:00"))
        except ValueError:
            return None
    return None


def get_event_time(event: dict[str, Any] | None, fallback: datetime) -> datetime:
    if not event:
        return fallback
    return (
        parse_time(event.get("timestamp"))
        or parse_time(event.get("t"))
        or parse_time(event.get("time"))
        or fallback
    )


def seconds_between(a: datetime | None, b: datetime | None) -> int:
    if not a or not b:
        return 0
    return max(0, int((b - a).total_seconds()))


def get_pos(event: dict[str, Any]) -> tuple[float | None, float | None, int | None]:
    pos_x = event.get("pos_x", event.get("x"))
    pos_z = event.get("pos_z", event.get("z"))
    floor_id = event.get("floor_id", event.get("floor"))

    pos = event.get("pos") or event.get("position")
    if isinstance(pos, dict):
        pos_x = pos_x if pos_x is not None else pos.get("x")
        pos_z = pos_z if pos_z is not None else pos.get("z")
        floor_id = floor_id if floor_id is not None else pos.get("floor_id", pos.get("floor"))
    elif isinstance(pos, (list, tuple)):
        if len(pos) >= 1 and pos_x is None:
            pos_x = pos[0]
        if len(pos) >= 3 and pos_z is None:
            pos_z = pos[2]
        elif len(pos) >= 2 and pos_z is None:
            pos_z = pos[1]

    try:
        x = float(pos_x) if pos_x is not None else None
        z = float(pos_z) if pos_z is not None else None
    except (TypeError, ValueError):
        return None, None, None

    try:
        floor = int(floor_id) if floor_id is not None else None
    except (TypeError, ValueError):
        floor = None

    return x, z, floor


def bool_from_event(value: Any) -> bool:
    if isinstance(value, bool):
        return value
    if isinstance(value, str):
        return value.lower() in {"1", "true", "yes", "y", "ai"}
    return bool(value)


def avg(values: list[float]) -> float:
    return round(sum(values) / len(values), 2) if values else 0.0

# ---------------------------------------------------------------------------
# Schema
# ---------------------------------------------------------------------------

def ensure_schema() -> None:
    def _do() -> bool:
        with pg_engine.begin() as conn:
            conn.execute(text(SCHEMA_SQL))
            conn.execute(text(MIGRATIONS_SQL))
        return True

    retry_with_backoff(_do, label="ensure_schema")
    print("[worker] Esquema PostgreSQL listo.")

# ---------------------------------------------------------------------------
# Procesamiento
# ---------------------------------------------------------------------------

def build_session_metrics(session: dict[str, Any]) -> dict[str, Any]:
    events = flatten_events(session.get("events") or [])
    now = datetime.now(timezone.utc)

    session_id = str(session.get("session_id") or session.get("match_id") or session.get("_id"))[:64]
    player_id = str(session.get("player_id") or session_id)[:64]

    sorted_events = sorted(events, key=lambda e: get_event_time(e, now))

    start_event = next((e for e in sorted_events if event_type(e) == EVENT_SESSION_START), None)
    end_event = next((e for e in reversed(sorted_events) if event_type(e) == EVENT_SESSION_END), None)

    started_at = get_event_time(start_event, now) if start_event else (
        get_event_time(sorted_events[0], now) if sorted_events else now
    )
    ended_at = get_event_time(end_event, started_at) if end_event else (
        get_event_time(sorted_events[-1], started_at) if sorted_events else None
    )

    kills = sum(1 for e in sorted_events if event_type(e) == EVENT_KILL)
    deaths = sum(1 for e in sorted_events if event_type(e) == EVENT_DEATH)
    shots_fired = sum(1 for e in sorted_events if event_type(e) == EVENT_SHOT)
    shots_hit = sum(1 for e in sorted_events if event_type(e) == EVENT_HIT)
    items_picked = sum(1 for e in sorted_events if event_type(e) == EVENT_ITEM)

    kd_ratio = round(kills / deaths, 4) if deaths > 0 else float(kills)
    accuracy = round(shots_hit / shots_fired, 4) if shots_fired > 0 else 0.0
    duration_seconds = seconds_between(started_at, ended_at)

    death_rows: list[dict[str, Any]] = []
    position_rows: list[dict[str, Any]] = []
    item_rows: list[dict[str, Any]] = []

    last_spawn_at: datetime | None = started_at
    ttl_values: list[float] = []

    for event in sorted_events:
        ev_type = event_type(event)
        occurred_at = get_event_time(event, started_at)

        if ev_type in {EVENT_SPAWN, EVENT_SESSION_START}:
            last_spawn_at = occurred_at
            continue

        if ev_type == EVENT_DEATH:
            pos_x, pos_z, floor_id = get_pos(event)
            ttl_seconds = (
                max(0.0, (occurred_at - last_spawn_at).total_seconds())
                if last_spawn_at is not None
                else None
            )
            if ttl_seconds is not None:
                ttl_values.append(ttl_seconds)

            if pos_x is not None and pos_z is not None:
                death_rows.append({
                    "session_id": session_id,
                    "player_id": str(event.get("player_id") or player_id)[:64],
                    "pos_x": pos_x,
                    "pos_z": pos_z,
                    "floor_id": floor_id,
                    "killer_id": (str(event.get("killer_id"))[:64] if event.get("killer_id") else None),
                    "is_ai": bool_from_event(event.get("is_ai", False)),
                    "ttl_seconds": ttl_seconds,
                    "occurred_at": occurred_at,
                })
            last_spawn_at = None
            continue

        if ev_type == EVENT_POSITION:
            pos_x, pos_z, floor_id = get_pos(event)
            if pos_x is not None and pos_z is not None:
                position_rows.append({
                    "session_id": session_id,
                    "player_id": str(event.get("player_id") or player_id)[:64],
                    "pos_x": pos_x,
                    "pos_z": pos_z,
                    "floor_id": floor_id,
                    "recorded_at": occurred_at,
                })
            continue

        if ev_type == EVENT_ITEM:
            item_rows.append({
                "session_id": session_id,
                "player_id": str(event.get("player_id") or player_id)[:64],
                "item_type": str(event.get("item_type") or event.get("item") or "Unknown")[:32],
                "occurred_at": occurred_at,
            })

    return {
        "session_id": session_id,
        "player_id": player_id,
        "started_at": started_at,
        "ended_at": ended_at,
        "duration_seconds": duration_seconds,
        "kills": kills,
        "deaths": deaths,
        "kd_ratio": kd_ratio,
        "accuracy": accuracy,
        "avg_ttl_seconds": avg(ttl_values),
        "shots_fired": shots_fired,
        "shots_hit": shots_hit,
        "items_picked": items_picked,
        "death_rows": death_rows,
        "position_rows": position_rows,
        "item_rows": item_rows,
    }


def write_metrics(metrics: dict[str, Any]) -> None:
    player_stub = text("""
        INSERT INTO player_stats (player_id)
        VALUES (:player_id)
        ON CONFLICT (player_id) DO NOTHING;
    """)

    player_upsert = text("""
        INSERT INTO player_stats (
            player_id, total_kills, total_deaths, kd_ratio, avg_accuracy,
            avg_ttl_seconds, total_sessions, total_playtime_seconds,
            items_picked, last_updated
        )
        VALUES (
            :player_id, :kills, :deaths, :kd_ratio, :accuracy,
            :avg_ttl_seconds, 1, :duration_seconds, :items_picked, NOW()
        )
        ON CONFLICT (player_id) DO UPDATE SET
            total_kills = player_stats.total_kills + EXCLUDED.total_kills,
            total_deaths = player_stats.total_deaths + EXCLUDED.total_deaths,
            total_sessions = player_stats.total_sessions + 1,
            total_playtime_seconds = player_stats.total_playtime_seconds + EXCLUDED.total_playtime_seconds,
            items_picked = player_stats.items_picked + EXCLUDED.items_picked,
            kd_ratio = CASE
                WHEN (player_stats.total_deaths + EXCLUDED.total_deaths) = 0
                THEN (player_stats.total_kills + EXCLUDED.total_kills)::NUMERIC
                ELSE ROUND(
                    (player_stats.total_kills + EXCLUDED.total_kills)::NUMERIC /
                    NULLIF(player_stats.total_deaths + EXCLUDED.total_deaths, 0),
                    4
                )
            END,
            avg_accuracy = ROUND(
                (
                    ((player_stats.avg_accuracy::NUMERIC * player_stats.total_sessions) + EXCLUDED.avg_accuracy::NUMERIC) /
                    NULLIF(player_stats.total_sessions + 1, 0)
                )::NUMERIC,
                4
            ),
            avg_ttl_seconds = ROUND(
                (
                    ((player_stats.avg_ttl_seconds::NUMERIC * player_stats.total_sessions) + EXCLUDED.avg_ttl_seconds::NUMERIC) /
                    NULLIF(player_stats.total_sessions + 1, 0)
                )::NUMERIC,
                2
            ),
            last_updated = NOW();
    """)

    session_upsert = text("""
        INSERT INTO session_stats (
            session_id, player_id, started_at, ended_at, duration_seconds,
            kills, deaths, kd_ratio, accuracy, avg_ttl_seconds,
            shots_fired, shots_hit, items_picked
        )
        VALUES (
            :session_id, :player_id, :started_at, :ended_at, :duration_seconds,
            :kills, :deaths, :kd_ratio, :accuracy, :avg_ttl_seconds,
            :shots_fired, :shots_hit, :items_picked
        )
        ON CONFLICT (session_id) DO NOTHING;
    """)

    insert_death = text("""
        INSERT INTO death_events (
            session_id, player_id, pos_x, pos_z, floor_id,
            killer_id, is_ai, ttl_seconds, occurred_at
        ) VALUES (
            :session_id, :player_id, :pos_x, :pos_z, :floor_id,
            :killer_id, :is_ai, :ttl_seconds, :occurred_at
        );
    """)

    insert_position = text("""
        INSERT INTO position_events (
            session_id, player_id, pos_x, pos_z, floor_id, recorded_at
        ) VALUES (
            :session_id, :player_id, :pos_x, :pos_z, :floor_id, :recorded_at
        );
    """)

    insert_item = text("""
        INSERT INTO item_events (session_id, player_id, item_type, occurred_at)
        VALUES (:session_id, :player_id, :item_type, :occurred_at);
    """)

    def _do() -> bool:
        with pg_engine.begin() as conn:
            # Primero garantizamos que existe player_stats para que session_stats no rompa la FK.
            # Esta fila minima no suma metricas.
            conn.execute(player_stub, {"player_id": metrics["player_id"]})

            # Si la sesion ya existe, salimos sin volver a sumar ni duplicar eventos.
            result = conn.execute(session_upsert, metrics)
            if result.rowcount == 0:
                print(f"[worker] Sesion {metrics['session_id']} ya existe en session_stats; no se duplica.")
                return True

            # Solo si la sesion es nueva acumulamos metricas del jugador y guardamos eventos.
            conn.execute(player_upsert, metrics)
            if metrics["death_rows"]:
                conn.execute(insert_death, metrics["death_rows"])
            if metrics["position_rows"]:
                conn.execute(insert_position, metrics["position_rows"])
            if metrics["item_rows"]:
                conn.execute(insert_item, metrics["item_rows"])
        return True

    retry_with_backoff(_do, label=f"write_metrics({metrics['session_id']})")


def process_session(mongo_id: str) -> None:
    try:
        oid = ObjectId(mongo_id)
    except InvalidId:
        print(f"[worker] WARN: ObjectId invalido recibido desde Redis: {mongo_id}")
        return

    session = events_col.find_one({"_id": oid})
    if session is None:
        print(f"[worker] WARN: sesion {mongo_id} no encontrada en Mongo.")
        return

    if session.get("processed"):
        print(f"[worker] Sesion {mongo_id} ya procesada, ignorando.")
        return

    metrics = build_session_metrics(session)

    print(
        f"[worker] Procesando session={metrics['session_id']} player={metrics['player_id']} | "
        f"kills={metrics['kills']} deaths={metrics['deaths']} "
        f"accuracy={metrics['accuracy']:.2%} ttl={metrics['avg_ttl_seconds']:.2f}s "
        f"items={metrics['items_picked']}"
    )

    write_metrics(metrics)

    events_col.update_one(
        {"_id": oid},
        {"$set": {"processed": True, "processed_at": datetime.now(timezone.utc)}},
    )

    print(f"[worker] OK: sesion {mongo_id} procesada y guardada en PostgreSQL.")


def recover_unprocessed(limit: int = 100) -> int:
    count = 0
    cursor = events_col.find({"processed": {"$ne": True}}, {"_id": 1}).limit(limit)
    for doc in cursor:
        redis_client.rpush(REDIS_QUEUE_KEY, str(doc["_id"]))
        count += 1
    if count:
        print(f"[worker] Recuperadas {count} sesiones pendientes hacia Redis.")
    return count

# ---------------------------------------------------------------------------
# Main
# ---------------------------------------------------------------------------

def main() -> None:
    print("[worker] Iniciando Worker de Metricas...")
    print(f"[worker] Mongo collection: {MONGO_DB_NAME}.{MONGO_COLLECTION}")
    print(f"[worker] Redis queue     : {REDIS_HOST}:{REDIS_PORT}/{REDIS_DB} -> {REDIS_QUEUE_KEY}")

    time.sleep(2)
    ensure_schema()

    try:
        recover_unprocessed()
    except Exception as exc:
        print(f"[worker] WARN: no se pudo recuperar backlog Mongo: {type(exc).__name__}: {exc}")

    print(f"[worker] Escuchando cola Redis '{REDIS_QUEUE_KEY}' ...")

    while True:
        try:
            item = redis_client.blpop(REDIS_QUEUE_KEY, timeout=5)
            if item is None:
                continue
            _, mongo_id = item
            process_session(mongo_id)

        except KeyboardInterrupt:
            print("[worker] Detenido por el usuario.")
            break
        except redis.exceptions.ConnectionError as exc:
            print(f"[worker] Redis caido: {exc}. Reintento en 3s...")
            time.sleep(3)
        except Exception as exc:
            print(f"[worker] ERROR inesperado: {type(exc).__name__}: {exc}")
            time.sleep(3)


if __name__ == "__main__":
    main()
