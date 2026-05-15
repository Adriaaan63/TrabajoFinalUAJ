"""Configuración centralizada de la Query API.

Las variables se leen una sola vez desde el entorno (o el .env) y se exponen
a través de un objeto Settings inmutable. Evita tener os.getenv() repartido
por el código y centraliza la validación.
"""
from functools import lru_cache

from pydantic import Field
from pydantic_settings import BaseSettings, SettingsConfigDict


class Settings(BaseSettings):
    model_config = SettingsConfigDict(env_file=".env", extra="ignore")

    # --- PostgreSQL ---------------------------------------------------------
    # URL completa (postgresql://user:pass@host:port/db). El worker escribe
    # aquí; la Query API solo lee.
    postgres_url: str = Field(..., alias="POSTGRES_URL")

    # Pool de conexiones: dimensionado para una API de lectura con tráfico
    # moderado. pool_recycle evita el "stale connection" típico de bases en
    # la nube que cierran conexiones inactivas.
    db_pool_size: int = Field(5, alias="DB_POOL_SIZE")
    db_max_overflow: int = Field(10, alias="DB_MAX_OVERFLOW")
    db_pool_recycle_seconds: int = Field(1800, alias="DB_POOL_RECYCLE")

    # --- Servidor -----------------------------------------------------------
    host: str = Field("0.0.0.0", alias="QUERY_API_HOST")
    port: int = Field(8001, alias="QUERY_API_PORT")

    # --- CORS ---------------------------------------------------------------
    # Orígenes desde los que el frontend puede llamar a la API. En desarrollo
    # típicamente localhost:3000 (CRA) o localhost:5173 (Vite).
    cors_origins: list[str] = Field(
        default=[
            "http://localhost:3000",
            "http://localhost:5173",
            "http://127.0.0.1:3000",
            "http://127.0.0.1:5173",
        ],
        alias="CORS_ORIGINS",
    )

    # --- Logging ------------------------------------------------------------
    log_level: str = Field("INFO", alias="LOG_LEVEL")


@lru_cache
def get_settings() -> Settings:
    """Devuelve la instancia singleton de Settings."""
    return Settings()