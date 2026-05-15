"""Conexión a PostgreSQL con SQLAlchemy Core.

La Query API es de SOLO LECTURA, así que no necesitamos un ORM completo:
usamos `text()` con parámetros nombrados, que es:
  - Más rápido que mapear a clases ORM.
  - Inmune a inyección SQL (los parámetros se pasan separados del texto).
  - Más explícito sobre qué columnas se devuelven.

El engine se crea perezosamente la primera vez que se pide, y se cierra
ordenadamente en el shutdown de FastAPI.
"""
import logging
from contextlib import contextmanager
from typing import Iterator

from sqlalchemy import create_engine, text
from sqlalchemy.engine import Connection, Engine

from config import get_settings

logger = logging.getLogger(__name__)

_engine: Engine | None = None


def get_engine() -> Engine:
    """Devuelve (creando si hace falta) el engine global."""
    global _engine
    if _engine is None:
        settings = get_settings()
        _engine = create_engine(
            settings.postgres_url,
            pool_size=settings.db_pool_size,
            max_overflow=settings.db_max_overflow,
            pool_recycle=settings.db_pool_recycle_seconds,
            pool_pre_ping=True,  # detecta y descarta conexiones muertas
            future=True,
        )
        logger.info("Engine de PostgreSQL inicializado (pool=%s)", settings.db_pool_size)
    return _engine


@contextmanager
def db_connection() -> Iterator[Connection]:
    """Context manager para obtener una conexión del pool.

    Uso:
        with db_connection() as conn:
            rows = conn.execute(text("SELECT ..."), {...}).mappings().all()
    """
    engine = get_engine()
    with engine.connect() as conn:
        yield conn


def ping() -> bool:
    """Comprueba que PostgreSQL está accesible. Usado por /health."""
    try:
        with db_connection() as conn:
            conn.execute(text("SELECT 1"))
        return True
    except Exception as exc:
        logger.warning("Ping a PostgreSQL falló: %s", exc)
        return False


def dispose_engine() -> None:
    """Cierra el pool. Se llama en el shutdown de la app."""
    global _engine
    if _engine is not None:
        _engine.dispose()
        _engine = None
        logger.info("Engine de PostgreSQL cerrado")