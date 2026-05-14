# 🚀 Guía de Despliegue: Pipeline de Telemetría con Docker

Este documento explica cómo levantar toda la infraestructura de análisis (Bases de datos, APIs y Workers) utilizando Docker de forma correcta.

## 1. Requisitos Previos
* Tener instalado **Docker Desktop**.
* Tener una terminal abierta (PowerShell, CMD o Terminal de VS Code) en la carpeta raíz del proyecto (`...\Assets\Docker\`).

---

## 2. Estructura de Archivos (Crítico)
Para que el comando de Docker funcione y encuentre el código, la estructura de carpetas debe ser **exactamente** la siguiente. Si un nombre cambia, Docker no encontrará el "contexto" y fallará.

```text
/Docker (Carpeta Raíz)
├── docker-compose.yml
├── /ingest_api
│     └── Dockerfile
├── /metrics_worker
│     └── Dockerfile
└── /query_api
      └── Dockerfile

```

---

## 3. Comandos de Despliegue

Ejecuta estos comandos en tu terminal dentro de la carpeta `/Docker`:

### A. Construir y levantar todo (Recomendado)

Este comando lee el archivo `.yml`, crea las redes y construye tus contenedores personalizados.

```bash
docker-compose up --build
```

### B. Levantar en segundo plano (Modo "Silencioso")

Si prefieres que la terminal se quede libre y gestionar todo desde la interfaz visual de Docker Desktop:

```bash
docker-compose up -d --build
```

### C. Detener el sistema

Para apagar todos los contenedores sin borrar los datos de las bases de datos:

```bash
docker-compose down
```

---

## 4. Uso de la Interfaz (Docker Desktop)

Una vez ejecutado el comando, abre **Docker Desktop**:

1. **Containers:** Verás un grupo con el nombre de tu carpeta. Al desplegarlo, verás los 6 servicios (`db-events`, `db-stats`, `redis-server`, `ingest-api`, `metrics-worker`, `query-api`).
2. **Estado:** Todos deben tener un **círculo verde (Running)**.
3. **Logs:** Si haces clic en el nombre de un contenedor, verás la pestaña "Logs". Allí aparecerán los `print()` de tus scripts de Python y cualquier error de conexión.

---

## 5. Resolución de Problemas Comunes

### ❌ Error: "path ... not found" o "unable to prepare context"

* **Causa:** Docker busca una carpeta que no existe o está mal escrita en el `docker-compose.yml`.
* **Solución:** Revisa que el nombre de la carpeta física coincida con la línea `build: ./nombre_carpeta`.

### ❌ Error: "port is already allocated" (Puerto ocupado)

* **Causa:** Ya tienes instalado MongoDB o PostgreSQL localmente en Windows.
* **Solución:** Cierra esos programas o servicios de Windows, o cambia el puerto izquierdo en el `docker-compose.yml` (ej. de `"5432:5432"` a `"5433:5432"`).

### ❌ El contenedor se pone en Gris/Rojo inmediatamente

* **Causa:** El script de Python tiene un error de sintaxis o falta una librería.
* **Solución:** Mira los **Logs** en Docker Desktop para ver el "Traceback" del error de Python.

---

## 6. URLs de Acceso Local

* **Ingest API (Unity):** `http://localhost:8000/docs`
* **Query API (Web):** `http://localhost:8001/docs`
* **MongoDB:** `localhost:27017`
* **PostgreSQL:** `localhost:5432`