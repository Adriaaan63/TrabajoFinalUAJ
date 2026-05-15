-- =====================================================================
-- ESQUEMA POSTGRESQL ESPERADO POR LA QUERY API
-- =====================================================================
-- Este archivo NO se ejecuta desde la Query API. Es el "contrato" que
-- debe cumplir el metrics_worker cuando vuelca sus agregados.
--
-- Si los nombres de tablas/columnas que produce el worker son distintos,
-- bastará con ajustar las queries SQL en query_api/routers/*.py. Todas
-- son SQL plano y explícito.
--
-- Convención:
--   · player_stats    → 1 fila por jugador  (acumulado de toda su historia)
--   · session_stats   → 1 fila por partida  (resumen por session_id)
--   · death_events    → 1 fila por muerte   (para heatmaps + TTL)
--   · position_events → 1 fila cada 5s      (Player_Position_Heartbeat)
--   · item_events     → 1 fila por recogida (Item_Picked)
-- =====================================================================


-- Acumulado por jugador (refrescado por el worker tras cada sesión)
CREATE TABLE IF NOT EXISTS player_stats (
    player_id              VARCHAR(64)   PRIMARY KEY,
    total_kills            INTEGER       NOT NULL DEFAULT 0,
    total_deaths           INTEGER       NOT NULL DEFAULT 0,
    kd_ratio               NUMERIC(10,4) NOT NULL DEFAULT 0,  -- M3.1
    avg_accuracy           NUMERIC(5,4)  NOT NULL DEFAULT 0,  -- M3.2  (0–1)
    avg_ttl_seconds        NUMERIC(8,2)  NOT NULL DEFAULT 0,  -- M1.1
    total_sessions         INTEGER       NOT NULL DEFAULT 0,
    total_playtime_seconds INTEGER       NOT NULL DEFAULT 0,
    items_picked           INTEGER       NOT NULL DEFAULT 0,  -- M4.1
    last_updated           TIMESTAMPTZ   NOT NULL DEFAULT NOW()
);
-- El lookup por player_id ya usa el índice de la PK; no se necesitan otros.


-- Resumen por sesión
CREATE TABLE IF NOT EXISTS session_stats (
    session_id        VARCHAR(64)   PRIMARY KEY,
    player_id         VARCHAR(64)   NOT NULL REFERENCES player_stats(player_id) ON DELETE CASCADE,
    started_at        TIMESTAMPTZ   NOT NULL,
    ended_at          TIMESTAMPTZ,
    duration_seconds  INTEGER       NOT NULL DEFAULT 0,
    kills             INTEGER       NOT NULL DEFAULT 0,
    deaths            INTEGER       NOT NULL DEFAULT 0,
    kd_ratio          NUMERIC(10,4) NOT NULL DEFAULT 0,
    accuracy          NUMERIC(5,4)  NOT NULL DEFAULT 0,
    avg_ttl_seconds   NUMERIC(8,2)  NOT NULL DEFAULT 0,
    shots_fired       INTEGER       NOT NULL DEFAULT 0,
    shots_hit         INTEGER       NOT NULL DEFAULT 0,
    items_picked      INTEGER       NOT NULL DEFAULT 0
);

-- Acelera /players/{id}/sessions y /players/{id}/progression
CREATE INDEX IF NOT EXISTS idx_session_player_time
    ON session_stats (player_id, started_at DESC);


-- Eventos de muerte (mantenidos en Postgres para heatmaps + TTL)
CREATE TABLE IF NOT EXISTS death_events (
    id            BIGSERIAL        PRIMARY KEY,
    session_id    VARCHAR(64)      NOT NULL,
    player_id     VARCHAR(64),                        -- NULL si murió la IA y no se identifica
    pos_x         DOUBLE PRECISION NOT NULL,
    pos_z         DOUBLE PRECISION NOT NULL,
    floor_id      INTEGER,
    killer_id     VARCHAR(64),
    is_ai         BOOLEAN          NOT NULL DEFAULT FALSE,  -- TRUE = murió un agente IA
    ttl_seconds   NUMERIC(8,2),                       -- M1.1: spawn → death
    occurred_at   TIMESTAMPTZ      NOT NULL
);

CREATE INDEX IF NOT EXISTS idx_death_floor   ON death_events (floor_id);
CREATE INDEX IF NOT EXISTS idx_death_session ON death_events (session_id);
CREATE INDEX IF NOT EXISTS idx_death_player  ON death_events (player_id);


-- Heartbeats de posición (cada 5s mientras el jugador está vivo)
CREATE TABLE IF NOT EXISTS position_events (
    id           BIGSERIAL        PRIMARY KEY,
    session_id   VARCHAR(64)      NOT NULL,
    player_id    VARCHAR(64)      NOT NULL,
    pos_x        DOUBLE PRECISION NOT NULL,
    pos_z        DOUBLE PRECISION NOT NULL,
    floor_id     INTEGER,
    recorded_at  TIMESTAMPTZ      NOT NULL
);

CREATE INDEX IF NOT EXISTS idx_position_floor  ON position_events (floor_id);
CREATE INDEX IF NOT EXISTS idx_position_player ON position_events (player_id);


-- Eventos de recogida de objetos (M4.1)
CREATE TABLE IF NOT EXISTS item_events (
    id           BIGSERIAL    PRIMARY KEY,
    session_id   VARCHAR(64)  NOT NULL,
    player_id    VARCHAR(64)  NOT NULL,
    item_type    VARCHAR(32)  NOT NULL,  -- 'Health' | 'Weapon' | 'Ammo'
    occurred_at  TIMESTAMPTZ  NOT NULL
);

CREATE INDEX IF NOT EXISTS idx_item_player ON item_events (player_id);
CREATE INDEX IF NOT EXISTS idx_item_type   ON item_events (item_type);