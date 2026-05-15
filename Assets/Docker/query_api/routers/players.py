"""Datos por jugador.

Tres endpoints que componen el "Tracker" del frontend:
  - /players/{id}              → tarjeta con stats acumuladas
  - /players/{id}/sessions     → historial de partidas
  - /players/{id}/progression  → curva temporal (alimenta H3)
"""
from fastapi import APIRouter, HTTPException, Path, Query, status
from sqlalchemy import text

from database import db_connection
from schemas import PlayerProfile, ProgressionPoint, SessionSummary

router = APIRouter(prefix="/players", tags=["players"])


@router.get(
    "/{player_id}",
    response_model=PlayerProfile,
    summary="Perfil agregado del jugador",
)
async def get_player_profile(
    player_id: str = Path(..., min_length=1, max_length=64),
) -> PlayerProfile:
    sql = text("""
        SELECT
            player_id,
            total_kills,
            total_deaths,
            kd_ratio,
            avg_accuracy,
            avg_ttl_seconds,
            total_sessions,
            total_playtime_seconds,
            items_picked
        FROM player_stats
        WHERE player_id = :player_id
    """)

    with db_connection() as conn:
        row = conn.execute(sql, {"player_id": player_id}).mappings().first()

    if row is None:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail=f"Jugador '{player_id}' no encontrado",
        )

    return PlayerProfile(
        player_id=row["player_id"],
        total_kills=row["total_kills"] or 0,
        total_deaths=row["total_deaths"] or 0,
        kd_ratio=float(row["kd_ratio"] or 0),
        avg_accuracy=float(row["avg_accuracy"] or 0),
        avg_ttl_seconds=float(row["avg_ttl_seconds"] or 0),
        total_sessions=row["total_sessions"] or 0,
        total_playtime_seconds=row["total_playtime_seconds"] or 0,
        items_picked=row["items_picked"] or 0,
    )


@router.get(
    "/{player_id}/sessions",
    response_model=list[SessionSummary],
    summary="Historial de sesiones del jugador",
)
async def get_player_sessions(
    player_id: str = Path(..., min_length=1, max_length=64),
    limit: int = Query(20, ge=1, le=200),
    offset: int = Query(0, ge=0),
) -> list[SessionSummary]:
    sql = text("""
        SELECT
            session_id,
            started_at,
            ended_at,
            duration_seconds,
            kills,
            deaths,
            kd_ratio,
            accuracy,
            avg_ttl_seconds,
            shots_fired,
            shots_hit,
            items_picked
        FROM session_stats
        WHERE player_id = :player_id
        ORDER BY started_at DESC
        LIMIT :limit OFFSET :offset
    """)

    with db_connection() as conn:
        rows = conn.execute(
            sql,
            {"player_id": player_id, "limit": limit, "offset": offset},
        ).mappings().all()

    return [
        SessionSummary(
            session_id=r["session_id"],
            started_at=r["started_at"].isoformat(),
            ended_at=r["ended_at"].isoformat() if r["ended_at"] else None,
            duration_seconds=r["duration_seconds"] or 0,
            kills=r["kills"] or 0,
            deaths=r["deaths"] or 0,
            kd_ratio=float(r["kd_ratio"] or 0),
            accuracy=float(r["accuracy"] or 0),
            avg_ttl_seconds=float(r["avg_ttl_seconds"] or 0),
            shots_fired=r["shots_fired"] or 0,
            shots_hit=r["shots_hit"] or 0,
            items_picked=r["items_picked"] or 0,
        )
        for r in rows
    ]


@router.get(
    "/{player_id}/progression",
    response_model=list[ProgressionPoint],
    summary="Curva temporal de K/D y precisión (valida H3)",
)
async def get_player_progression(
    player_id: str = Path(..., min_length=1, max_length=64),
    limit: int = Query(50, ge=1, le=500),
) -> list[ProgressionPoint]:
    """Devuelve las últimas N sesiones del jugador en orden cronológico.

    El frontend pinta directamente esta lista en un gráfico de líneas
    (Recharts), donde el eje X es session_number y los ejes Y son K/D y
    accuracy. Si la pendiente es positiva → H3 corroborada.
    """
    sql = text("""
        SELECT
            session_id,
            started_at,
            kd_ratio,
            accuracy,
            avg_ttl_seconds,
            ROW_NUMBER() OVER (ORDER BY started_at ASC) AS session_number
        FROM session_stats
        WHERE player_id = :player_id
        ORDER BY started_at ASC
        LIMIT :limit
    """)

    with db_connection() as conn:
        rows = conn.execute(sql, {"player_id": player_id, "limit": limit}).mappings().all()

    return [
        ProgressionPoint(
            session_number=int(r["session_number"]),
            session_id=r["session_id"],
            played_at=r["started_at"].isoformat(),
            kd_ratio=float(r["kd_ratio"] or 0),
            accuracy=float(r["accuracy"] or 0),
            avg_ttl_seconds=float(r["avg_ttl_seconds"] or 0),
        )
        for r in rows
    ]