"""Endpoint de salud.

Útil para:
  - Healthcheck del propio contenedor Docker (ver Dockerfile).
  - Sonda de Kubernetes / orquestador.
  - Verificar manualmente desde un navegador que la conexión a Postgres
    funciona.
"""
from datetime import datetime, timezone

from fastapi import APIRouter, status
from fastapi.responses import JSONResponse

from database import ping
from schemas import HealthStatus

router = APIRouter(tags=["health"])

API_VERSION = "1.0.0"


@router.get("/health")
async def health_check() -> JSONResponse:
    """Devuelve 200 si la BD responde, 503 si está caída."""
    db_ok = ping()
    payload = HealthStatus(
        status="ok" if db_ok else "degraded",
        database="up" if db_ok else "down",
        version=API_VERSION,
        timestamp=datetime.now(timezone.utc).isoformat(),
    )
    code = status.HTTP_200_OK if db_ok else status.HTTP_503_SERVICE_UNAVAILABLE
    return JSONResponse(payload.model_dump(), status_code=code)