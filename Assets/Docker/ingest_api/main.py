import os
from fastapi import FastAPI
from pymongo import MongoClient

app = FastAPI()

# Leemos la URL de la nube desde el archivo .env (que Docker inyecta)
MONGO_URL = os.getenv("MONGO_URL")
client = MongoClient(MONGO_URL)
db = client["telemetry_db"]
events_collection = db["raw_events"]

@app.get("/")
def check_connection():
    try:
        # Hacemos un test rápido a Mongo en la nube
        client.admin.command('ping')
        return {"status": "ok", "message": "¡Conectado a MongoDB Atlas en la nube!"}
    except Exception as e:
        return {"status": "error", "message": str(e)}

# El comando real para arrancar FastAPI
if __name__ == "__main__":
    import uvicorn
    uvicorn.run(app, host="0.0.0.0", port=8000)