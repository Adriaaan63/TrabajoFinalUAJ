import os
import json
import redis
from fastapi import FastAPI, Request, HTTPException
from pymongo import MongoClient
import datetime

# Ingest API -> web - http://localhost:8000/docs

app = FastAPI()

# --- Conexiones ---
MONGO_URL = os.getenv("MONGO_URL")
client = MongoClient(MONGO_URL)
db = client["telemetry_db"]
events_collection = db["raw_events"]

# Redis local (nombre del servicio en docker-compose)
redis_client = redis.Redis(host='redis-server', port=6379, db=0, decode_responses=True)

REDIS_QUEUE_KEY = "sessions_to_process"

@app.get("/")
def check_connection():
    try:
        client.admin.command('ping')
        return {"status": "ok", "message": "¡Conectado a MongoDB Atlas en la nube!"}
    except Exception as e:
        return {"status": "error", "message": str(e)}


@app.post("/upload_session")
async def upload_session(request: Request):
    """
    Recibe la sesion completa desde Unity (via DockerPersistence.cs).
    1. Guarda el JSON crudo en MongoDB como respaldo permanente.
    2. Encola el session_id en Redis para que el worker lo procese.
    """
    try:
        data = await request.json()
    except Exception:
        raise HTTPException(status_code=400, detail="JSON invalido o cuerpo vacio.")

    # Validacion minima: necesitamos al menos session_id y events
    if "session_id" not in data or "events" not in data:
        raise HTTPException(
            status_code=422,
            detail="El cuerpo debe contener 'session_id' y 'events'."
        )

    # Metadatos del servidor
    data["received_at"] = datetime.datetime.utcnow().isoformat()
    data["processed"] = False

    # 1. Guardar en MongoDB (fuente de verdad cruda)
    result = events_collection.insert_one(data)
    inserted_id = str(result.inserted_id)

    # 2. Encolar el _id de Mongo en Redis para que el worker lo procese
    #    Usamos rpush para añadir al final de la lista (FIFO con blpop por la izquierda)
    redis_client.rpush(REDIS_QUEUE_KEY, inserted_id)

    print(f"[ingest-api] Sesion recibida: {data['session_id']} | "
          f"Eventos: {len(data['events'])} | Mongo _id: {inserted_id}")

    return {
        "status": "success",
        "message": "Sesion almacenada y encolada para procesamiento.",
        "session_id": data["session_id"],
        "events_received": len(data["events"])
    }


if __name__ == "__main__":
    import uvicorn
    uvicorn.run(app, host="0.0.0.0", port=8000)