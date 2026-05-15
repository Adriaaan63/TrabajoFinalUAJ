import os
from fastapi import FastAPI
from sqlalchemy import create_engine, text

# Query API -> web - http://localhost:8001/docs

app = FastAPI()

# Leemos la URL de Postgres en la nube
POSTGRES_URL = os.getenv("POSTGRES_URL")
# SQLAlchemy necesita que el prefijo empiece por postgresql://
engine = create_engine(POSTGRES_URL)

@app.get("/")
def check_connection():
    try:
        with engine.connect() as conn:
            conn.execute(text("SELECT 1"))
        return {"status": "ok", "message": "¡Conectado a PostgreSQL en la nube!"}
    except Exception as e:
        return {"status": "error", "message": str(e)}

if __name__ == "__main__":
    import uvicorn
    uvicorn.run(app, host="0.0.0.0", port=8001)