"""Punto de entrada de la Query API.

Lugar en el pipeline:
    Unity → Ingest API → MongoDB → Worker → PostgreSQL → [Query API] → Web

Esta API es de SOLO LECTURA y sirve dos bloques de datos:

  Bloque 1 · TRACKER DEL JUGADOR
      /api/v1/players/*       → consumido por el frontend React.
                                 Muestra al usuario su propio perfil que
                                 se actualiza tras cada partida.

  Bloque 2 · ANÁLISIS / INVESTIGACIÓN
      /api/v1/heatmaps/*      → distribuciones espaciales (H2)
      /api/v1/metrics/*       → TTL, items, resumen global (H1, H4)
                                 consumido por el dashboard interno para
                                 validar/refutar las hipótesis del plan.

Documentación interactiva en /docs (Swagger) y /redoc.
"""
import logging
from contextlib import asynccontextmanager

from fastapi import FastAPI, Request, status
from fastapi.exceptions import RequestValidationError
from fastapi.middleware.cors import CORSMiddleware
from fastapi.responses import JSONResponse
from sqlalchemy.exc import SQLAlchemyError

from config import get_settings
from database import dispose_engine, get_engine
from routers import health, heatmaps, metrics, players

# --- Logging ---------------------------------------------------------------
# Configuración temprana, antes de cualquier import que cree loggers.
logging.basicConfig(
    level=get_settings().log_level,
    format="%(asctime)s | %(levelname)-7s | %(name)s | %(message)s",
)
logger = logging.getLogger("query_api")


# --- Lifespan (startup/shutdown) ------------------------------------------
@asynccontextmanager
async def lifespan(app: FastAPI):
    settings = get_settings()
    logger.info("Arrancando Query API en %s:%s", settings.host, settings.port)
    # Forzamos crear el engine al arrancar: si la URL es inválida o la BD
    # no está accesible, fallamos rápido en vez de esperar a la primera
    # petición.
    get_engine()
    yield
    logger.info("Cerrando Query API")
    dispose_engine()


# --- App -------------------------------------------------------------------
app = FastAPI(
    title="FPS Telemetry · Query API",
    description=(
        "API de consulta de solo lectura sobre PostgreSQL.\n\n"
        "**Tracker** (`/players/*`) alimenta el frontend con el perfil del "
        "jugador. **Análisis** (`/heatmaps/*`, `/metrics/*`) alimenta el "
        "dashboard interno para validar las hipótesis del plan."
    ),
    version="1.0.0",
    lifespan=lifespan,
    docs_url="/docs",
    redoc_url="/redoc",
)

# --- Middleware ------------------------------------------------------------
# Sin CORS el navegador bloquea las peticiones del frontend React.
app.add_middleware(
    CORSMiddleware,
    allow_origins=get_settings().cors_origins,
    allow_credentials=False,
    allow_methods=["GET"],   # API de solo lectura
    allow_headers=["*"],
)


# --- Error handlers globales ----------------------------------------------
@app.exception_handler(SQLAlchemyError)
async def db_error_handler(request: Request, exc: SQLAlchemyError) -> JSONResponse:
    # Logueamos el stacktrace pero no lo exponemos al cliente.
    logger.exception("Error de BD en %s %s", request.method, request.url.path)
    return JSONResponse(
        status_code=status.HTTP_503_SERVICE_UNAVAILABLE,
        content={"detail": "Error al consultar la base de datos"},
    )


@app.exception_handler(RequestValidationError)
async def validation_error_handler(
    request: Request, exc: RequestValidationError
) -> JSONResponse:
    return JSONResponse(
        status_code=status.HTTP_422_UNPROCESSABLE_ENTITY,
        content={"detail": "Parámetros inválidos", "errors": exc.errors()},
    )


# --- Routers ---------------------------------------------------------------
# /health se queda fuera del prefijo /api/v1 para que sea sencillo de
# alcanzar desde el HEALTHCHECK del contenedor.
app.include_router(health.router)

# Bloque 1: Tracker del jugador
app.include_router(players.router, prefix="/api/v1")

# Bloque 2: Análisis / investigación
app.include_router(heatmaps.router, prefix="/api/v1")
app.include_router(metrics.router, prefix="/api/v1")


# --- Entry point local -----------------------------------------------------
if __name__ == "__main__":
    import uvicorn

    settings = get_settings()
    uvicorn.run(
        "main:app",
        host=settings.host,
        port=settings.port,
        reload=False,
    )