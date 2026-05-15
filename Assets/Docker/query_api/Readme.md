# Query API — FPS Telemetry

API de consulta de solo lectura para el pipeline de telemetría FPS.

```
Unity ─▶ Ingest API ─▶ MongoDB ─▶ Worker ─▶ PostgreSQL ─▶ [Query API] ─▶ Web
```

La API expone **dos bloques de datos** alineados con el plan inicial:

### Bloque 1 · Tracker del jugador
Datos que el frontend React enseña al usuario sobre sí mismo. Se actualizan tras cada partida que juega.

### Bloque 2 · Análisis / investigación
Datos agregados que consume el dashboard interno para validar/refutar las hipótesis del plan (H1, H2, H4).

---

## Estructura

```
query_api/
├── main.py                  # FastAPI app: middleware, lifespan, routers
├── config.py                # Settings con pydantic-settings
├── database.py              # Engine + pool + context manager
├── schemas.py               # Modelos Pydantic de respuesta
├── routers/
│   ├── __init__.py
│   ├── health.py            # GET /health
│   ├── players.py           # Bloque 1: Tracker
│   ├── heatmaps.py          # Bloque 2: Análisis (H2)
│   └── metrics.py           # Bloque 2: Análisis (H1, H4)
├── sql/
│   └── schema_reference.sql # Contrato de tablas que espera la API
├── Dockerfile
├── requirements.txt
└── README.md
```

---

## Endpoints

Documentación interactiva en `http://localhost:8001/docs` (Swagger UI) o `/redoc`.

### Bloque 1 · Tracker del jugador

| Método | Ruta                                      | Soporta | Para qué                                |
|--------|-------------------------------------------|---------|------------------------------------------|
| GET    | `/api/v1/players/{player_id}`             | —       | Perfil acumulado (K/D, accuracy, TTL...) |
| GET    | `/api/v1/players/{player_id}/sessions`    | —       | Historial de partidas paginado           |
| GET    | `/api/v1/players/{player_id}/progression` | **H3**  | Curva temporal K/D + accuracy            |

### Bloque 2 · Análisis / investigación

| Método | Ruta                                  | Soporta  | Métrica       |
|--------|---------------------------------------|----------|---------------|
| GET    | `/api/v1/heatmaps/deaths`             | **H2**   | M2.2          |
| GET    | `/api/v1/heatmaps/navigation`         | **H2**   | M2.1          |
| GET    | `/api/v1/metrics/ttl-distribution`    | **H1**   | M1.1          |
| GET    | `/api/v1/metrics/item-interactions`   | **H4**   | M4.1          |
| GET    | `/api/v1/metrics/global-summary`      | Contexto | Agregado      |

### Salud

| Método | Ruta      | Para qué                                       |
|--------|-----------|------------------------------------------------|
| GET    | `/health` | Healthcheck (también verifica conexión a BD)   |

### Parámetros comunes

- **Paginación:** `?limit=N&offset=M`
- **Filtros de heatmap:** `?floor_id=1&session_id=...&player_id=...&include_ai_deaths=true`
- **Bucket de TTL:** `?bucket_size_seconds=5`

---

## Puesta en marcha

### 1. Configurar `.env` (en la raíz del proyecto, no aquí dentro)

```env
MONGO_URL=mongodb+srv://USUARIO:CONTRASEÑA@cluster/...
POSTGRES_URL=postgresql://USUARIO:CONTRASEÑA@host:5432/db?sslmode=require
```

### 2. Lanzar el esquema de PostgreSQL una sola vez

```bash
psql "$POSTGRES_URL" -f query_api/sql/schema_reference.sql
```

### 3. Levantar con Docker Compose

```bash
docker compose up --build query-api
```

Disponible en `http://localhost:8001`. Visita `/docs` para probar interactivamente.

### 4. Desarrollo local sin Docker

```bash
cd query_api
pip install -r requirements.txt
uvicorn main:app --reload --port 8001
```

---

## Decisiones técnicas

### SQL parametrizado en todas las consultas
Todas las queries usan `text("... :param ...")` con valores pasados como diccionario aparte. Eso impide inyección SQL aunque un atacante envíe un `player_id` malicioso.

### Sin pandas
Para SELECTs simples es overhead innecesario. Se usa `connection.execute(...).mappings().all()`, que devuelve dicts directamente.

### Pool de conexiones con `pool_pre_ping`
Las bases en la nube (como Supabase) cierran conexiones inactivas. `pool_pre_ping=True` ejecuta un `SELECT 1` antes de devolver cada conexión, descartando las muertas. Combinado con `pool_recycle=1800`, el pool se mantiene fresco.

### CORS configurado
Sin CORS, el navegador bloquea las llamadas desde React. Por defecto se aceptan `localhost:3000` (CRA) y `localhost:5173` (Vite). Para producción, edita `CORS_ORIGINS` en el `.env`.

### Errores controlados
`SQLAlchemyError` y `RequestValidationError` se interceptan con handlers globales. El cliente recibe siempre un JSON `{"detail": "..."}` en vez de un stacktrace.

---

## Contrato con el worker

El `metrics_worker` debe escribir en las tablas definidas en `sql/schema_reference.sql`:

- **`player_stats`** y **`session_stats`** → upsert tras procesar cada sesión.
- **`death_events`**, **`position_events`**, **`item_events`** → append-only (un INSERT por evento original).
- **`ttl_seconds`** en `death_events` lo calcula el worker como `death.timestamp - last_spawn.timestamp` para ese `(player_id, session_id)`.

Si el worker usa nombres distintos, basta con ajustar las queries en `routers/*.py`; no hay ORM mapeado que romper.