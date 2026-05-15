# Metrics Worker

Servicio batch de la pipeline FPS Telemetry.

## Variables de entorno

```env
MONGO_URL=mongodb+srv://usuario:password@cluster/...
POSTGRES_URL=postgresql://usuario:password@host:5432/db?sslmode=require
MONGO_DB_NAME=telemetry_db
MONGO_COLLECTION=raw_events
REDIS_HOST=redis-server
REDIS_PORT=6379
REDIS_QUEUE_KEY=sessions_to_process
```

## Eventos esperados por defecto

- `Player_Kill`
- `Player_Death`
- `Player_Shot`
- `Player_Hit`
- `Player_Spawn`
- `Session_Start`
- `Session_End`
- `Player_Position_Heartbeat`
- `Item_Picked`

Puedes sobreescribirlos con variables `EVENT_KILL`, `EVENT_DEATH`, etc.

## Docker Compose

```yaml
metrics-worker:
  build: ./metrics_worker
  env_file: .env
  depends_on:
    - redis-server
  restart: on-failure
```

El worker crea/migra el esquema necesario en PostgreSQL al arrancar y recupera sesiones `processed != true` desde Mongo si Redis perdió la cola.
