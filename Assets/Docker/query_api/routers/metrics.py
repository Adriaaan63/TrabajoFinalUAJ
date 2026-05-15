"""Métricas de investigación.

Endpoints orientados al dashboard interno (no al frontend de jugadores).
Cada uno está pensado para validar/refutar una hipótesis concreta del
plan inicial:

  /metrics/ttl-distribution   → H1 (frustración por mortalidad temprana)
  /metrics/item-interactions  → H4 (economía de recursos)
  /metrics/global-summary     → contexto general del playtest
"""
from fastapi import APIRouter, Query
from sqlalchemy import text

from database import db_connection
from schemas import (
    GlobalSummary,
    ItemInteractionCount,
    ItemInteractionResponse,
    TtlBucket,
    TtlDistribution,
)

router = APIRouter(prefix="/metrics", tags=["research"])


@router.get(
    "/ttl-distribution",
    response_model=TtlDistribution,
    summary="Histograma de Time-to-Live (valida H1)",
)
async def get_ttl_distribution(
    player_id: str | None = Query(None, description="Filtra por jugador concreto"),
    bucket_size_seconds: int = Query(
        5, ge=1, le=60, description="Tamaño de cada bucket en segundos"
    ),
) -> TtlDistribution:
    """Devuelve la distribución de TTL en buckets de tamaño configurable.

    Si la mayor parte de las muertes se concentra en los primeros buckets
    (0–5s, 5–10s), la hipótesis de "castigo desproporcionado por IA
    agresiva" queda respaldada empíricamente.
    """
    where = "ttl_seconds IS NOT NULL"
    params: dict[str, object] = {"bucket": bucket_size_seconds}
    if player_id is not None:
        where += " AND player_id = :player_id"
        params["player_id"] = player_id

    buckets_sql = text(f"""
        SELECT
            FLOOR(ttl_seconds / :bucket) * :bucket AS bucket_start,
            COUNT(*) AS death_count
        FROM death_events
        WHERE {where}
        GROUP BY bucket_start
        ORDER BY bucket_start ASC
    """)
    stats_sql = text(f"""
        SELECT
            COUNT(*) AS total,
            AVG(ttl_seconds) AS mean_value,
            PERCENTILE_CONT(0.5) WITHIN GROUP (ORDER BY ttl_seconds) AS median_value
        FROM death_events
        WHERE {where}
    """)

    with db_connection() as conn:
        bucket_rows = conn.execute(buckets_sql, params).mappings().all()
        stats = conn.execute(stats_sql, params).mappings().first()

    buckets = [
        TtlBucket(
            bucket_start_seconds=float(r["bucket_start"]),
            bucket_end_seconds=float(r["bucket_start"]) + bucket_size_seconds,
            count=int(r["death_count"]),
        )
        for r in bucket_rows
    ]

    return TtlDistribution(
        bucket_size_seconds=bucket_size_seconds,
        total_deaths=int(stats["total"]) if stats else 0,
        mean_seconds=(
            float(stats["mean_value"])
            if stats and stats["mean_value"] is not None
            else None
        ),
        median_seconds=(
            float(stats["median_value"])
            if stats and stats["median_value"] is not None
            else None
        ),
        buckets=buckets,
    )


@router.get(
    "/item-interactions",
    response_model=ItemInteractionResponse,
    summary="Recogidas de objetos por tipo (valida H4)",
)
async def get_item_interactions(
    player_id: str | None = Query(None),
) -> ItemInteractionResponse:
    """Cuenta agrupada por tipo de objeto (Health/Weapon/Ammo).

    Si la mayoría de jugadores ignora los objetos secundarios y se queda
    con el arma inicial, H4 queda corroborada.
    """
    where = "TRUE"
    params: dict[str, object] = {}
    if player_id is not None:
        where = "player_id = :player_id"
        params["player_id"] = player_id

    sql = text(f"""
        SELECT item_type, COUNT(*) AS pickup_count
        FROM item_events
        WHERE {where}
        GROUP BY item_type
        ORDER BY pickup_count DESC
    """)

    with db_connection() as conn:
        rows = conn.execute(sql, params).mappings().all()

    by_type = [
        ItemInteractionCount(
            item_type=str(r["item_type"]),
            pickup_count=int(r["pickup_count"]),
        )
        for r in rows
    ]
    return ItemInteractionResponse(
        total_pickups=sum(c.pickup_count for c in by_type),
        by_item_type=by_type,
    )


@router.get(
    "/global-summary",
    response_model=GlobalSummary,
    summary="Resumen agregado para el dashboard interno",
)
async def get_global_summary() -> GlobalSummary:
    """Métricas globales del playtest. Útil como cabecera del dashboard
    de investigación (ej. "X jugadores, Y sesiones, K/D medio = Z")."""
    sql = text("""
        SELECT
            (SELECT COUNT(*) FROM player_stats)  AS total_players,
            (SELECT COUNT(*) FROM session_stats) AS total_sessions,
            (SELECT COUNT(*) FROM death_events)  AS total_deaths,
            COALESCE(AVG(kd_ratio), 0)           AS avg_kd,
            COALESCE(AVG(avg_accuracy), 0)       AS avg_accuracy,
            COALESCE(AVG(avg_ttl_seconds), 0)    AS avg_ttl
        FROM player_stats
    """)

    with db_connection() as conn:
        row = conn.execute(sql).mappings().first()

    return GlobalSummary(
        total_players=int(row["total_players"] or 0),
        total_sessions=int(row["total_sessions"] or 0),
        total_deaths=int(row["total_deaths"] or 0),
        global_avg_kd=float(row["avg_kd"] or 0),
        global_avg_accuracy=float(row["avg_accuracy"] or 0),
        global_avg_ttl_seconds=float(row["avg_ttl"] or 0),
    )