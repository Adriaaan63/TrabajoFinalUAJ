import os
import time
import json
import redis
from bson import ObjectId
from pymongo import MongoClient
from sqlalchemy import create_engine, text

# --- Conexiones ---
MONGO_URL    = os.getenv("MONGO_URL")
POSTGRES_URL = os.getenv("POSTGRES_URL")

mongo_client  = MongoClient(MONGO_URL)
db            = mongo_client["telemetry_db"]
events_col    = db["raw_events"]

pg_engine = create_engine(POSTGRES_URL)

# Redis local (nombre del servicio en docker-compose)
redis_client = redis.Redis(host='redis-server', port=6379, db=0, decode_responses=True)

REDIS_QUEUE_KEY = "sessions_to_process"   # debe coincidir con ingest_api/main.py

# -------------------------------------------------------------------

def ensure_table():
    """Crea la tabla player_stats si no existe todavia."""
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
    with pg_engine.connect() as conn:
        conn.execute(text(ddl))
        conn.commit()


def process_session(mongo_id: str):
    """
    Lee una sesion de MongoDB por su _id, calcula las metricas
    y hace un UPSERT en PostgreSQL.
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

    # --- UPSERT en PostgreSQL ---
    # Si el jugador ya existe: acumula kills/deaths/shots/hits,
    # recalcula precision como media ponderada por sesiones y actualiza last_seen.
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

    # Marcar como procesada en Mongo para evitar doble proceso
    events_col.update_one(
        {"_id": ObjectId(mongo_id)},
        {"$set": {"processed": True}}
    )

    print(f"[worker] OK: sesion {mongo_id} procesada y guardada en PostgreSQL.")


def main():
    print("[worker] Iniciando Worker de Metricas...")
    print(f"[worker] Mongo  : {MONGO_URL[:30]}...")
    print(f"[worker] Postgres: {POSTGRES_URL[:30]}...")

    # Esperamos a que Redis este listo (puede tardar un par de segundos al arrancar)
    time.sleep(2)
    ensure_table()

    print(f"[worker] Escuchando cola Redis '{REDIS_QUEUE_KEY}' ...")

    while True:
        try:
            # blpop bloquea hasta que llega un item (timeout=5 para poder hacer Ctrl+C limpio)
            item = redis_client.blpop(REDIS_QUEUE_KEY, timeout=5)
            if item is None:
                # Timeout sin datos: volvemos a escuchar
                continue

            _, mongo_id = item   # blpop devuelve (nombre_lista, valor)
            process_session(mongo_id)

        except KeyboardInterrupt:
            print("[worker] Detenido por el usuario.")
            break
        except Exception as e:
            print(f"[worker] ERROR: {e}")
            time.sleep(3)   # Pausa antes de reintentar para no saturar logs


if __name__ == "__main__":
    main()