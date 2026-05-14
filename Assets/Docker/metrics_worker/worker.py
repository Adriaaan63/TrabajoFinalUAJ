import os
import time
import redis
from pymongo import MongoClient
from sqlalchemy import create_engine

# Conexiones a la nube
MONGO_URL = os.getenv("MONGO_URL")
POSTGRES_URL = os.getenv("POSTGRES_URL")

# Conexión al Redis local (dentro de Docker)
redis_client = redis.Redis(host='redis-server', port=6379, db=0)

def main():
    print("Iniciando Worker de Métricas...")
    print(f"Mongo URL detectada: {MONGO_URL[:20]}...")
    print(f"Postgres URL detectada: {POSTGRES_URL[:20]}...")
    
    # Bucle infinito escuchando a Redis
    while True:
        print("Worker activo: esperando nuevos datos para procesar...")
        time.sleep(10) # Pausa de 10 segundos

if __name__ == "__main__":
    main()