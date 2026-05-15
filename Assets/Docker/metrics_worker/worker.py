import os
import time
import redis
from bson import ObjectId
from pymongo import MongoClient
from sqlalchemy import create_engine, text
from sqlalchemy.exc import OperationalError, DBAPIError

# --- Conexiones ---
MONGO_URL    = os.getenv("MONGO_URL")
POSTGRES_URL = os.getenv("POSTGRES_URL")

mongo_client = MongoClient(MONGO_URL)
db           = mongo_client["telemetry_db"]
events_col   = db["raw_events"]

# pool_pre_ping evita usar conexiones muertas tras un corte de red.
# pool_recycle recicla la conexion cada 30min para evitar timeouts del pooler.
pg_engine = create_engine(
    POSTGRES_URL,
    pool_pre_ping=True,
    pool_recycle=1800,
)

# Redis local (nombre del servicio en docker-compose)
redis_client = redis.Redis(host='redis-server', port=6379, db=0, decode_responses=True)

REDIS_QUEUE_KEY = "sessions_to_process"

# -------------------------------------------------------------------
# Helpers de reintento
# -------------------------------------------------------------------

def retry_with_backoff(fn, *, max_attempts=8, base_delay=2.0, max_delay=30.0, label=""):
    """
    Ejecuta fn() reintentando ante errores de red/DB con backoff exponencial.
    - max_attempts: numero maximo de intentos antes de rendirse.
    - base_delay: segundos iniciales de espera.
    - max_delay: techo del delay para no esperar eternamente.
    """
    attempt = 0
    delay = base_delay
    while True:
        attempt += 1
        try:
            return fn()
        except (OperationalError, DBAPIError) as e:
            if attempt >= max_attempts:
                print(f"[worker] {label} fallo tras {attempt} intentos: {e}")
                raise
            print(f"[worker] {label} intento {attempt}/{max_attempts} fallido "
                  f"({type(e).__name__}). Reintento en {delay:.1f}s...")
            time.sleep(delay)
            delay = min(delay * 2, max_delay)


# -------------------------------------------------------------------
# Logica de negocio
# -------------------------------------------------------------------

def ensure_table():
    """Crea la tabla player_stats si no existe todavia. Con retry."""
    ddl = """
    CREATE TABLE IF NOT EXISTS player_stats (
        player_id    TEXT PRIMARY KEY,
        total_kills  INTEGER NOT NULL DEFAULT 0,
        total_deaths INTEGER NOT NULL DEFAULT 0,
        total_shots  INTEGER NOT NULL DEFAULT 0,
        total_hits   INTEGER NOT NULL DEFAULT 0,
        avg_accuracy FLOAT   NOT NULL DEFAULT 0.0,
        sessions     INTEGER NOT NULL DEFAULT 0,
        last_seen    TIMESTAMP
    );
    """

    def _do():
        with pg_engine.connect() as conn:
            conn.execute(text(ddl))
            conn.commit()
        return True

    retry_with_backoff(_do, label="ensure_table")
    print("[worker] Tabla player_stats lista.")


def process_session(mongo_id: str):
    """
    Lee una sesion de MongoDB por su _id, calcula las metricas
    y hace un UPSERT en PostgreSQL. Con retry en la parte de Postgres.
    """
    session = events_col.find_one({"_id": ObjectId(mongo_id)})
    if session is None:
        print(f"[worker] WARN: sesion {mongo_id} no encontrada en Mongo.")
        return

    if session.get("processed"):
        print(f"[worker] Sesion {mongo_id} ya procesada, ignorando.")
        return

    player_id = session.get("player_id") or session.get("session_id", "unknown")
    events    = session.get("events", [])

    # --- Calculo de metricas ---
    kills  = sum(1 for e in events if e.get("event_type") == "Player_Kill")
    deaths = sum(1 for e in events if e.get("event_type") == "Player_Death")
    shots  = sum(1 for e in events if e.get("event_type") == "Player_Shot")
    hits   = sum(1 for e in events if e.get("event_type") == "Player_Hit")

    accuracy = round((hits / shots * 100), 2) if shots > 0 else 0.0

    print(f"[worker] Procesando player={player_id} | "
          f"kills={kills} deaths={deaths} precision={accuracy}%")

    upsert_sql = text("""
        INSERT INTO player_stats
            (player_id, total_kills, total_deaths, total_shots, total_hits,
             avg_accuracy, sessions, last_seen)
        VALUES
            (:player_id, :kills, :deaths, :shots, :hits,
             :accuracy, 1, NOW())
        ON CONFLICT (player_id) DO UPDATE SET
            total_kills  = player_stats.total_kills  + EXCLUDED.total_kills,
            total_deaths = player_stats.total_deaths + EXCLUDED.total_deaths,
            total_shots  = player_stats.total_shots  + EXCLUDED.total_shots,
            total_hits   = player_stats.total_hits   + EXCLUDED.total_hits,
            avg_accuracy = ROUND(
                (player_stats.avg_accuracy * player_stats.sessions + EXCLUDED.avg_accuracy)
                / (player_stats.sessions + 1)::FLOAT, 2
            ),
            sessions     = player_stats.sessions + 1,
            last_seen    = NOW();
    """)

    def _do_upsert():
        with pg_engine.connect() as conn:
            conn.execute(upsert_sql, {
                "player_id": player_id,
                "kills":     kills,
                "deaths":    deaths,
                "shots":     shots,
                "hits":      hits,
                "accuracy":  accuracy,
            })
            conn.commit()
        return True

    retry_with_backoff(_do_upsert, label=f"upsert({player_id})")

    # Marcar como procesada en Mongo
    events_col.update_one(
        {"_id": ObjectId(mongo_id)},
        {"$set": {"processed": True}}
    )

    print(f"[worker] OK: sesion {mongo_id} procesada y guardada en PostgreSQL.")


def main():
    print("[worker] Iniciando Worker de Metricas...")
    if MONGO_URL:
        print(f"[worker] Mongo  : {MONGO_URL[:30]}...")
    if POSTGRES_URL:
        print(f"[worker] Postgres: {POSTGRES_URL[:30]}...")

    # Damos margen a que la red del compose este lista
    time.sleep(2)

    # Si esto sigue fallando tras todos los reintentos, el contenedor sale
    # y Docker puede reiniciarlo (si configuras restart: on-failure).
    ensure_table()

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
        except redis.exceptions.ConnectionError as e:
            print(f"[worker] Redis caido: {e}. Reintento en 3s...")
            time.sleep(3)
        except Exception as e:
            # No matamos el worker por un error en una sesion suelta.
            print(f"[worker] ERROR inesperado: {type(e).__name__}: {e}")
            time.sleep(3)


if __name__ == "__main__":
    main()