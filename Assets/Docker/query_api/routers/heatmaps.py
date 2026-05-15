"""Heatmaps para investigación.

Sirven los datos espaciales que validan la Hipótesis 2 (mapa, choke points,
zonas seguras). El formato de salida es compatible con heatmap.js:
  [{x, z, value}, ...]

Filtros compartidos: floor_id, session_id, player_id. Todos son opcionales;
si no se pasan, se devuelve el agregado global limitado por `limit`.
"""
from fastapi import APIRouter, Query
from sqlalchemy import text

from database import db_connection
from schemas import HeatmapPoint, HeatmapResponse

router = APIRouter(prefix="/heatmaps", tags=["heatmaps"])


def _build_where(
    floor_id: int | None,
    session_id: str | None,
    player_id: str | None,
    extra: list[str] | None = None,
) -> tuple[str, dict[str, object]]:
    """Construye un WHERE seguro a partir de filtros opcionales.

    Solo añadimos placeholders (:nombre) al SQL; los valores se pasan
    por separado en `params`, así que es a prueba de inyección.
    """
    conditions: list[str] = ["TRUE"]
    params: dict[str, object] = {}

    if floor_id is not None:
        conditions.append("floor_id = :floor_id")
        params["floor_id"] = floor_id
    if session_id is not None:
        conditions.append("session_id = :session_id")
        params["session_id"] = session_id
    if player_id is not None:
        conditions.append("player_id = :player_id")
        params["player_id"] = player_id
    if extra:
        conditions.extend(extra)

    return " AND ".join(conditions), params


@router.get(
    "/deaths",
    response_model=HeatmapResponse,
    summary="Coordenadas de muertes (M2.2)",
)
async def get_deaths_heatmap(
    floor_id: int | None = Query(None, description="Filtra por planta del mapa"),
    session_id: str | None = Query(None),
    player_id: str | None = Query(None),
    include_ai_deaths: bool = Query(
        True, description="Si False, solo muertes de jugadores humanos"
    ),
    limit: int = Query(5000, ge=1, le=50000),
) -> HeatmapResponse:
    """Distribución espacial de mortalidad. Identifica los choke points
    donde se concentran las bajas (H2)."""
    extra = [] if include_ai_deaths else ["is_ai = FALSE"]
    where_clause, params = _build_where(floor_id, session_id, player_id, extra)
    params["limit"] = limit

    sql = text(f"""
        SELECT pos_x, pos_z
        FROM death_events
        WHERE {where_clause}
        LIMIT :limit
    """)

    with db_connection() as conn:
        rows = conn.execute(sql, params).mappings().all()

    points = [HeatmapPoint(x=float(r["pos_x"]), z=float(r["pos_z"])) for r in rows]
    return HeatmapResponse(floor_id=floor_id, point_count=len(points), points=points)


@router.get(
    "/navigation",
    response_model=HeatmapResponse,
    summary="Coordenadas de tránsito (M2.1)",
)
async def get_navigation_heatmap(
    floor_id: int | None = Query(None),
    session_id: str | None = Query(None),
    player_id: str | None = Query(None),
    limit: int = Query(10000, ge=1, le=100000),
) -> HeatmapResponse:
    """Heartbeats de posición del jugador. Revela los caminos preferidos
    frente a zonas evitadas (H2)."""
    where_clause, params = _build_where(floor_id, session_id, player_id)
    params["limit"] = limit

    sql = text(f"""
        SELECT pos_x, pos_z
        FROM position_events
        WHERE {where_clause}
        LIMIT :limit
    """)

    with db_connection() as conn:
        rows = conn.execute(sql, params).mappings().all()

    points = [HeatmapPoint(x=float(r["pos_x"]), z=float(r["pos_z"])) for r in rows]
    return HeatmapResponse(floor_id=floor_id, point_count=len(points), points=points)