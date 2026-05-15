"""Schemas Pydantic para las respuestas de la API.

Cada endpoint declara su `response_model` apuntando aquí, lo cual:
  - Valida que devolvemos exactamente lo que decimos.
  - Genera el esquema OpenAPI automáticamente (visible en /docs).
  - Filtra campos accidentales antes de serializar a JSON.

Los modelos están agrupados por bloque funcional:
  · SALUD              → health checks
  · TRACKER DEL JUGADOR → datos por jugador (consume el frontend React)
  · ANÁLISIS           → métricas para validar hipótesis (dashboard interno)
"""
from pydantic import BaseModel, Field


# =============================================================================
# SALUD / META
# =============================================================================

class HealthStatus(BaseModel):
    status: str = Field(..., examples=["ok", "degraded"])
    database: str = Field(..., examples=["up", "down"])
    version: str
    timestamp: str


# =============================================================================
# TRACKER DEL JUGADOR
# Datos que el frontend React muestra al usuario sobre sí mismo.
# =============================================================================

class PlayerProfile(BaseModel):
    """Perfil agregado del jugador, leído de player_stats."""
    player_id: str
    total_kills: int
    total_deaths: int
    kd_ratio: float = Field(..., description="K/D acumulado (M3.1)")
    avg_accuracy: float = Field(..., ge=0, le=1, description="Precisión 0–1 (M3.2)")
    avg_ttl_seconds: float = Field(..., description="Time-to-Live medio (M1.1)")
    total_sessions: int
    total_playtime_seconds: int
    items_picked: int = Field(..., description="Recogidas totales de objetos (M4.1)")


class SessionSummary(BaseModel):
    """Resumen de una sesión individual."""
    session_id: str
    started_at: str
    ended_at: str | None
    duration_seconds: int
    kills: int
    deaths: int
    kd_ratio: float
    accuracy: float
    avg_ttl_seconds: float
    shots_fired: int
    shots_hit: int
    items_picked: int


class ProgressionPoint(BaseModel):
    """Un punto en la curva temporal del jugador. Para validar H3."""
    session_number: int
    session_id: str
    played_at: str
    kd_ratio: float
    accuracy: float
    avg_ttl_seconds: float


# =============================================================================
# ANÁLISIS / INVESTIGACIÓN
# Datos agregados que consume el dashboard interno para validar hipótesis.
# =============================================================================

class HeatmapPoint(BaseModel):
    """Coordenada 2D compatible con heatmap.js.

    Unity usa Y como vertical y X/Z como plano horizontal, así que el mapa
    de calor del suelo se construye sobre X/Z.
    """
    x: float
    z: float
    value: float = Field(1.0, description="Intensidad del punto (por defecto 1)")


class HeatmapResponse(BaseModel):
    floor_id: int | None
    point_count: int
    points: list[HeatmapPoint]


class TtlBucket(BaseModel):
    """Un bucket del histograma de Time-to-Live."""
    bucket_start_seconds: float
    bucket_end_seconds: float
    count: int


class TtlDistribution(BaseModel):
    """Distribución de TTL agrupada en buckets. Valida H1."""
    bucket_size_seconds: int
    total_deaths: int
    mean_seconds: float | None
    median_seconds: float | None
    buckets: list[TtlBucket]


class ItemInteractionCount(BaseModel):
    item_type: str
    pickup_count: int


class ItemInteractionResponse(BaseModel):
    """Recuento de recogidas por tipo de objeto. Valida H4."""
    total_pickups: int
    by_item_type: list[ItemInteractionCount]


class GlobalSummary(BaseModel):
    """Métricas agregadas globales para el dashboard interno."""
    total_players: int
    total_sessions: int
    total_deaths: int
    global_avg_kd: float
    global_avg_accuracy: float
    global_avg_ttl_seconds: float