# DEATHMATCH
> [!IMPORTANT]
> Descomprimir el archivo [LightingData.asset](https://drive.google.com/drive/folders/18iWt0cuqHj0zVcg49CBBQVjHz7uMHkMN?usp=sharing),  a descargar, en la siguiente ruta del proyecto `/Assets/Opsive/DeathmatchAIKit/Demo/Scenes/Spark/LightingData.asset`.

# MEMORIA DEL PROYECTO: SISTEMA DE TELEMETRÍA FPS
---
<div align="center">
  <!-- Badges estilizadas para el repositorio de GitHub -->
  <img src="https://img.shields.io/badge/Asignatura-Usabilidad%20y%20An%C3%A1lisis%20de%20Juegos-purple?style=for-the-badge" alt="Asignatura">
  <img src="https://img.shields.io/badge/Curso-2025%2F2026-blue?style=for-the-badge" alt="Curso">
  <img src="https://img.shields.io/badge/Plataforma-Unity%20%2B%20Docker%20%2B%20React-emerald?style=for-the-badge" alt="Plataforma">
</div>

---

* **Título del Proyecto:** Sistema Dual de Telemetría y Análisis para Opsive FPS Deathmatch
* **Asignatura:** Usabilidad y Análisis de Juegos (Curso 2025/2026)
* **Número de Grupo:** Grupo 02
* **Integrantes:**
  * Marcos Pérez Martínez
  * Marcos Pantoja Rafael de la Cruz
  * Adrián Castellanos Ormeño
  * Sergio Pérez Robledano
  * Miguel Ángel López Muñoz

## ÍNDICE DE LA MEMORIA

1. [Breve Resumen del Trabajo](#1-breve-resumen-del-trabajo)
2. [Objetivos e Hipótesis de Evaluación](#2-objetivos-e-hip%C3%B3tesis-de-evaluaci%C3%B3n)
3. [Diseño e Implementación Técnica (Arquitectura Docker)](#3-dise%C3%B1o-e-implementaci%C3%B3n-t%C3%A9cnica-arquitectura-docker)
4. [Resultados Obtenidos (Validación Cuantitativa)](#4-resultados-obtenidos-validaci%C3%B3n-cuantitativa)
5. [Conclusiones y Fricciones de Diseño Detectadas](#5-conclusiones-y-fricciones-de-dise%C3%B1o-detectadas)
6. [Adenda: Registro Obligatorio de Reparto de Tareas](#-adenda-registro-obligatorio-de-reparto-de-tareas)

---

## DESARROLLO DE LA MEMORIA

### 1. Breve resumen del trabajo <a name="1-breve-resumen-del-trabajo"></a>

Este proyecto presenta el diseño e implementación de un sistema de telemetría avanzado para un videojuego FPS desarrollado en Unity, basado en el "Deathmatch AI Kit" de Opsive. El objetivo principal es la recolección y análisis de datos cuantitativos mediante una arquitectura en la nube para validar empíricamente cuatro hipótesis de diseño relacionadas con la agresividad de la IA, la navegación espacial, el impacto del metajuego competitivo y la economía de recursos.

Para lograrlo, se ha desarrollado un pipeline de datos completamente dockerizado compuesto por tres capas principales. La primera consta de una Ingest API (FastAPI) de alta velocidad que almacena los eventos crudos en un lago de datos NoSQL en MongoDB. La segunda capa actúa como motor de procesamiento asíncrono, utilizando Redis como gestor de colas y un Metrics Worker en Python que limpia y calcula las métricas, consolidando la información en una base de datos relacional PostgreSQL. Finalmente, una Query API de solo lectura sirve estos datos a una interfaz web (Frontend) construida con React, Vite y Tailwind CSS.

Esta interfaz se divide en dos módulos: el "Tracker del Jugador", que incentiva la retención visualizando la curva de aprendizaje mediante gráficas de Recharts, y un "Laboratorio de Investigación" exclusivo para diseñadores. Este último permite visualizar las zonas de conflicto del mapa superponiendo las coordenadas de mortalidad sobre imágenes cenitales del nivel mediante la librería simpleheat. En conjunto, el sistema proporciona una solución robusta que transforma eventos aislados en conocimiento analítico accionable.

---

### 2. Objetivos

En esta sección se detalla el propósito fundamental del proyecto, los objetivos técnicos y analíticos perseguidos, su integración con los conceptos teóricos de la asignatura y las expectativas establecidas para la validación del sistema.

#### 2.1. Propósito del Proyecto
El proyecto se ha realizado para construir un puente práctico entre el diseño de videojuegos y la ingeniería de datos. El propósito es trascender la evaluación cualitativa (basada en opiniones de *playtesting*) mediante la implementación de un sistema de telemetría cuantitativo completo. Esto permite recopilar evidencias empíricas sobre el comportamiento de los jugadores en un entorno FPS, transformando la interacción del usuario en métricas procesables que ayuden a la toma de decisiones de diseño.

#### 2.2. Objetivos Específicos
1. **Diseñar e implementar una arquitectura de telemetría robusta:** Desarrollar un modelo cliente-servidor para el registro de eventos (*Tracker*) que recoja datos estructurados en formato JSON (como coordenadas, identificadores de sesión y timestamps) sin penalizar el rendimiento del juego.
2. **Construir un sistema de evaluación dual:** Crear una capa visible (*Frontend Tracker*) orientada al jugador para fomentar la retención y la competitividad, y una capa analítica oculta (*Dashboard/Laboratorio*) orientada al equipo de desarrollo.
3. **Validar hipótesis de diseño mediante Game Analytics:** Utilizar las métricas extraídas para confirmar o refutar fricciones detectadas previamente, evaluando la presión de la IA, el uso del arsenal, y visualizando las zonas de conflicto real mediante métricas espaciales (mapas de calor).

#### 2.3. Alineación con los Contenidos de la Asignatura
El desarrollo del proyecto aplica directamente los conocimientos fundamentales impartidos durante el curso:
* **Framework MDA (Mecánicas, Dinámicas y Estéticas):** El proyecto evalúa cómo ciertas *Mecánicas* (agresividad de la IA, ubicación de recursos o visualización de estadísticas) generan *Dinámicas* específicas de comportamiento (estrategias de cobertura o juego agresivo), desembocando en *Estéticas* concretas como la frustración, el reto o la satisfacción por dominio.
* **Implementación de Trackers:** Se ha aplicado la teoría de arquitectura de telemetría, definiendo un diccionario de eventos unívocos (`Player_Spawn`, `Player_Death`, `Shot_Hit`) y estructurando el envío de trazas asíncronas desde el cliente hacia el servidor.
* **Analítica de Juegos (Game Analytics):** Se emplean KPIs fundamentales y métricas de rendimiento (como el *K/D Ratio*, precisión o *Time-to-Live*) junto con analítica espacial bidimensional para comprender el comportamiento del jugador en el mapa.

#### 2.4. Expectativas del Proyecto
A través de este sistema, se tienen las siguientes expectativas:
* **Demostrar empíricamente el impacto del metajuego:** Se espera comprobar mediante el seguimiento temporal que la exposición del jugador a sus propias métricas (como la precisión o el ratio de bajas) fomenta una curva de aprendizaje con pendiente positiva, validando la Hipótesis 3.
* **Identificar fallos de Level Design:** Se espera que la renderización de las métricas espaciales revele de forma visual e inequívoca los cuellos de botella del nivel (*choke points*) y áreas desaprovechadas, evidenciando si el mapa guía correctamente la acción.
* **Validación de escalabilidad:** Se espera probar que una arquitectura desacoplada basada en colas (Redis) y procesamiento asíncrono (Worker) es capaz de soportar la ingesta masiva de eventos típica de una fase de *Beta testing* real.

### 3. Diseño e Implementación Técnica (Arquitectura Docker) <a name="3-dise%C3%B1o-e-implementaci%C3%B3n-t%C3%A9cnica-arquitectura-docker"></a>

#### **3.1. Implementación del sistema de captura de eventos en el cliente Unity**

integrando el *Tracker* de telemetría con el framework Opsive Deathmatch AI Kit mediante el sistema de eventos nativos del motor.

##### **Definición del diccionario de eventos (`GameplayEvents.cs`)**

Se diseñaron e implementaron todas las clases de evento de gameplay del proyecto, cada una extendiendo la clase base `TrackerEvent` del sistema de telemetría. El diccionario cubre los siete eventos acordados en la especificación: `Player_Spawn`, `Player_Death`, `AI_Death`, `Shot_Fired`, `Shot_Hit`, `Player_Position_Heartbeat` e `Item_Picked`. Cada clase encapsula exclusivamente los atributos relevantes para su tipo (posición, identificador del killer, zona de impacto, tipo de ítem o nombre del arma), manteniendo los payloads compactos y coherentes con el esquema esperado por la API de ingesta.

##### **Captura de eventos del jugador (`PlayerEventHooks.cs`)**

Se implementó el script `PlayerEventHooks`, que se adjunta al prefab del jugador y se suscribe a tres eventos del `EventHandler` de Opsive: `OnDeath` para registrar `Player_Death` con posición y killer, `OnRespawn` para registrar `Player_Spawn` con las coordenadas X/Z de reaparición, y `OnItemUseComplete` para registrar `Shot_Fired` con el nombre del arma disparada. Adicionalmente, el script ejecuta una corutina interna que emite un evento `Player_Position_Heartbeat` cada cinco segundos con la posición actual del personaje, permitiendo reconstruir el recorrido del jugador a lo largo de la sesión.

##### **Captura de eventos de agentes IA (`AIEventHooks.cs`)**

Se implementó el script `AIEventHooks`, que se adjunta al prefab de cada agente enemigo. Se suscribe a `OnDeath` para registrar `AI_Death` con posición y killer, y a `OnHealthDamage` para detectar impactos recibidos. En este último caso, el handler filtra por el tag `Player` en el parámetro `attacker` antes de emitir `Shot_Hit`, evitando registrar daño entre agentes IA. La zona de impacto se infiere del nombre del `Collider` afectado, clasificando el golpe como `Head` o `Body`.

##### **Captura de recogida de ítems (`ItemPickupHooks.cs`)**

Se implementó el script `ItemPickupHooks`, que se adjunta a cada ítem del mapa y expone un campo configurable `itemType` en el Inspector de Unity. El script se suscribe al evento `OnItemPickup` del propio GameObject del ítem y registra `Item_Picked` únicamente cuando el parámetro `picker` corresponde a un objeto con el tag `Player`, descartando así cualquier interacción de la IA con los objetos del escenario.

##### **Resolución de incompatibilidades con el framework de Opsive**

Durante la integración se identificaron y resolvieron varias incompatibilidades entre la documentación del kit y su versión instalada. El `EventHandler` se encontraba bajo el namespace `Opsive.Shared.Events` y no bajo `Opsive.UltimateCharacterController.Events`. El evento `OnDeath` requería una firma `InvokableAction<Vector3, Vector3, GameObject>` en lugar de una `Action` simple, causando excepción en tiempo de ejecución con la firma incorrecta. El evento de daño recibido se denominaba `OnHealthDamage` en lugar de `OnHealthDamageReceived`. Finalmente, el evento de disparo `OnItemUseComplete` esperaba un parámetro de tipo `IUsableItem` en lugar de `System.Object`. Cada incompatibilidad se diagnosticó mediante scripts de detección temporal que registraban en consola los eventos y tipos reales lanzados por el framework, permitiendo corregir las firmas sin modificar código interno de Opsive.

---

#### **3.2. Implementación de la persistencia de Unity a Docker**

Implementación del subsistema de persistencia desde Unity hacia la infraestructura Docker, así como en la definición del esqueleto de la base de datos y el motor de procesamiento de métricas.

##### **Capa de persistencia en Unity (`DockerPersistence.cs`)**

Se diseñó e implementó la clase `DockerPersistence`, una nueva implementación de la interfaz `IPersistence` que permite al tracker de Unity enviar los datos de sesión directamente a la API de ingesta dockerizada. La clase adopta una estrategia de acumulación en memoria mediante un `StringBuilder` durante toda la partida, posponiendo el envío HTTP hasta el cierre limpio de la aplicación (`OnApplicationQuit`). Esto garantiza que el payload enviado contiene la sesión completa y es coherente con el endpoint `/upload_session` de la API. El envío se realiza de forma síncrona con `HttpClient`, siguiendo el mismo patrón que la persistencia de Firebase ya existente en el proyecto.

##### **Integración en el sistema de configuración (`TrackerConfig.cs` y `TrackerInitializer.cs`)**

Se extendió `TrackerConfig` con el nuevo campo `dockerApiUrl` para permitir configurar la URL del contenedor tanto desde el Inspector de Unity como desde el fichero externo `tracker.config.json`, sin necesidad de recompilar. En `TrackerInitializer` se añadió la opción `DOCKER` al enumerado `P_type`, el bloque de interfaz visual en el Inspector correspondiente y la lógica de instanciación en `ChoosePersistence()`, incluyendo un fallback automático a `LocalFile` si la URL está vacía. De esta forma la nueva persistencia queda completamente integrada en el ciclo de vida del tracker existente sin romper ninguna funcionalidad previa.

##### **API de ingesta (`ingest_api/main.py`)**

Se implementó el endpoint `POST /upload_session`, que era inexistente en el código original. El endpoint valida la presencia de los campos obligatorios `session_id` y `events`, añade metadatos de servidor (`received_at`, `processed: false`), persiste el documento completo en MongoDB Atlas como respaldo permanente, y encola el identificador de Mongo en Redis mediante `rpush` para notificar al worker de forma asíncrona.

##### **Motor de procesamiento de métricas (`metrics_worker/worker.py`)**

Se reescribió el worker desde el esqueleto vacío original hasta una implementación funcional completa. El worker escucha la cola Redis mediante `blpop` bloqueante, recupera la sesión de MongoDB por su `_id`, calcula las métricas de juego filtrando eventos por tipo y ejecuta un UPSERT en PostgreSQL que acumula estadísticas entre sesiones del mismo jugador con una media de precisión ponderada por número de partidas.

A continuación, se detalla la continuación de la **Sección 3: Diseño e Implementación Técnica**, centrada específicamente en la **Capa de Visualización y Análisis Web (Frontend)**. Este bloque está completamente desarrollado y estructurado en Markdown, utilizando terminología avanzada de ingeniería de software y analítica de juegos, incluyendo fragmentos de código críticos, listo para copiar y pegar directamente en tu archivo `README.md`.

---

#### **3.3. Implementación del Motor Analítico y Procesamiento Asíncrono de Métricas (`metrics_worker/worker.py`)**

La tercera capa de la arquitectura corresponde al sistema de procesamiento analítico asíncrono encargado de transformar los eventos crudos almacenados en MongoDB en métricas relacionales estructuradas listas para consumo desde la Query API y el frontend web.

A diferencia de la API de ingesta, cuyo objetivo es maximizar velocidad de escritura, el worker implementa una etapa de consolidación y enriquecimiento de datos orientada a análisis cuantitativo, métricas longitudinales y generación de heatmaps espaciales.

##### **A. Arquitectura de Procesamiento Desacoplado**

El motor analítico se diseñó siguiendo una arquitectura basada en colas para desacoplar completamente el videojuego del procesamiento pesado de métricas:

```text
Unity
  ↓
Ingest API
  ↓
MongoDB (Raw Events)
  ↓
Redis Queue
  ↓
Metrics Worker
  ↓
PostgreSQL
````

El sistema utiliza Redis como intermediario FIFO entre la capa de ingesta y el motor de cálculo, permitiendo absorber ráfagas masivas de eventos sin bloquear la ejecución del juego ni la API principal.

##### **B. Escucha Reactiva mediante Redis (`BLPOP`)**

El worker permanece en escucha continua sobre la cola `sessions_to_process` utilizando la operación bloqueante `BLPOP`:

```python
item = redis_client.blpop(REDIS_QUEUE_KEY, timeout=5)
```

Este enfoque evita ciclos de polling activos y reduce el consumo innecesario de CPU mientras no existan sesiones pendientes.

Cuando una nueva sesión es insertada por la Ingest API, Redis desbloquea inmediatamente el worker para iniciar el procesamiento.

##### **C. Recuperación Automática tras Fallos (`recover_unprocessed`)**

Para evitar pérdida de datos ante reinicios inesperados del sistema, el worker implementa un mecanismo de recuperación automática:

```python
recover_unprocessed()
```

Durante el arranque, el motor consulta MongoDB buscando documentos cuyo campo:

```json
"processed": true
```

no exista o sea falso. Todas las sesiones pendientes son reinyectadas automáticamente en Redis.

Este mecanismo convierte la arquitectura en un sistema resiliente frente a:

* Reinicios de Docker.
* Caídas de PostgreSQL.
* Reinicios de Redis.
* Errores temporales de red.

##### **D. Normalización y Limpieza de Eventos**

Uno de los principales retos fue la heterogeneidad de estructuras enviadas desde Unity. Para resolverlo, se implementó un sistema de aplanado recursivo:

```python
flatten_events(raw)
```

La función admite:

* Eventos individuales.
* Arrays de eventos.
* Arrays anidados arbitrariamente.

De esta forma, el worker puede procesar cualquier estructura serializada desde el cliente sin modificar la lógica analítica.

##### **E. Resolución Temporal y Ordenación Cronológica**

Todos los eventos son normalizados temporalmente utilizando:

```python
parse_time(...)
```

El parser soporta:

* Timestamps Unix.
* Unix Miliseconds.
* ISO8601.
* Objetos `datetime`.

Posteriormente, los eventos se ordenan cronológicamente:

```python
sorted_events = sorted(events, key=lambda e: get_event_time(e, now))
```

Esto permite reconstruir la secuencia exacta de gameplay independientemente del orden original de llegada.

##### **F. Resolución Inteligente del `player_id`**

El sistema implementa una estrategia jerárquica para identificar correctamente al jugador:

1. Evento explícito `Player_Id`.
2. Campo raíz `player_id` en MongoDB.
3. Fallback automático al `session_id`.

```python
find_player_id_from_events(events)
```

Este diseño garantiza compatibilidad hacia atrás con sesiones antiguas y evita corrupción de datos históricos.

##### **G. Cálculo de Métricas de Combate**

El worker calcula automáticamente:

* Número de kills.
* Número de muertes.
* Accuracy.
* K/D Ratio.
* Tiempo jugado.
* Time-To-Live medio.
* Número de objetos recogidos.

###### **Derivación de kills mediante `AI_Death`**

El sistema no utiliza un evento explícito `Player_Kill`. En su lugar, las bajas del jugador se infieren analizando eventos `AI_Death` cuyo `killer_id` coincide con el jugador activo:

```python
kills = sum(
    1 for e in sorted_events
    if event_type(e) == EVENT_AI_DEATH
    and str(e.get("killer_id") or "") == player_id
)
```

Esto reduce redundancia de eventos y evita inconsistencias entre sistemas cliente-servidor.

###### **Cálculo de Accuracy**

La precisión ofensiva se calcula mediante:

```python
accuracy = round(shots_hit / shots_fired, 4)
```

La métrica se almacena en formato decimal normalizado (`0.0 → 1.0`) para facilitar cálculos estadísticos posteriores.

##### **H. Cálculo del Time-To-Live (TTL)**

Para validar hipótesis relacionadas con frustración y agresividad de IA, el worker calcula automáticamente el tiempo de supervivencia del jugador entre eventos `Player_Spawn` y `Player_Death`:

```python
ttl_seconds = (
    max(0.0, (occurred_at - last_spawn_at).total_seconds())
)
```

Cada muerte almacena individualmente su TTL asociado, permitiendo construir histogramas de supervivencia desde PostgreSQL.

##### **I. Construcción de Heatmaps Espaciales**

El worker genera automáticamente estructuras espaciales listas para visualización térmica.

###### **1. Heatmaps de Mortalidad (`death_events`)**

Se almacenan:

* Coordenadas X/Z.
* Planta del mapa.
* Killer.
* Tipo de muerte.
* TTL asociado.

Estas métricas permiten detectar:

* Choke points.
* Zonas peligrosas.
* Desequilibrios de navegación.

###### **2. Heatmaps de Navegación (`position_events`)**

Los eventos `Player_Position_Heartbeat` registran la posición periódica del jugador:

```python
EVENT_POSITION = "Player_Position_Heartbeat"
```

Esto permite reconstruir rutas reales de navegación y detectar áreas ignoradas del mapa.

###### **3. Economía de Recursos (`item_events`)**

Las recogidas de objetos se almacenan individualmente:

```python
EVENT_ITEM = "Item_Picked"
```

permitiendo estudiar el comportamiento económico de los jugadores sobre:

* Munición.
* Curación.
* Armamento.

##### **J. Persistencia Relacional y Sistema UPSERT**

El worker consolida toda la información en PostgreSQL mediante múltiples tablas analíticas:

| Tabla             | Función                          |
| ----------------- | -------------------------------- |
| `player_stats`    | Estadísticas globales acumuladas |
| `session_stats`   | Resumen por sesión               |
| `death_events`    | Heatmaps de mortalidad           |
| `position_events` | Heatmaps de navegación           |
| `item_events`     | Economía de recursos             |

La actualización histórica se realiza mediante operaciones UPSERT:

```sql
ON CONFLICT (player_id) DO UPDATE SET
```

Esto permite:

* Acumular sesiones.
* Recalcular medias globales.
* Mantener consistencia temporal.
* Evitar duplicados.

##### **K. Prevención de Duplicados y Consistencia**

Antes de persistir eventos, el sistema verifica si la sesión ya existe:

```python
if result.rowcount == 0:
```

Si la sesión ya fue procesada previamente:

* No se vuelven a sumar estadísticas.
* No se insertan eventos repetidos.
* No se recalculan métricas históricas.

Esto garantiza consistencia analítica incluso bajo reintentos automáticos.

##### **L. Gestión Robusta de Errores**

El worker implementa un sistema de reintentos con backoff exponencial:

```python
retry_with_backoff(...)
```

permitiendo tolerar:

* Caídas temporales de PostgreSQL.
* Timeouts de red.
* Desconexiones del pool SQLAlchemy.

Además, el engine SQL utiliza:

```python
pool_pre_ping=True
pool_recycle=1800
```

para detectar conexiones muertas automáticamente y reciclar conexiones antiguas.

##### **M. Integración Completa con Docker**

El motor analítico se ejecuta como un contenedor independiente dentro del clúster Docker:

* `metrics-worker`
* `redis-server`
* `ingest-api`
* `query-api`

La comunicación entre servicios se realiza mediante red interna Docker, garantizando:

* Modularidad.
* Escalabilidad horizontal.
* Reproducibilidad del entorno.
* Despliegue portable.

En conjunto, esta capa transforma eventos aislados de gameplay en conocimiento analítico estructurado listo para explotación visual y validación cuantitativa de hipótesis de diseño.

---
---
#### **3.4. Implementación de la Capa de Consulta y Servicio de Datos (`query_api/`)**

La cuarta capa de la arquitectura corresponde al microservicio de lectura encargado de exponer al mundo exterior los agregados relacionales que el motor analítico ha consolidado en PostgreSQL. Mientras que la Ingest API se especializa en absorber escrituras y el worker se centra en transformar datos crudos en métricas, la Query API adopta un rol exclusivamente de servicio: traduce filas de PostgreSQL en respuestas JSON estructuradas, validadas y autodocumentadas, listas para ser consumidas tanto por el frontend del jugador como por el dashboard interno de investigación.

Su diseño parte de una premisa explícita: **es una API de solo lectura**. Esta decisión elimina la necesidad de transacciones complejas, colas de tareas o estado mutable, lo que se traduce en una arquitectura más sencilla, mayor rendimiento por petición y un perfil de seguridad reducido.

##### **A. Arquitectura Modular del Servicio**

El microservicio se estructura siguiendo una división estricta de responsabilidades por archivo, evitando ficheros monolíticos y facilitando el mantenimiento futuro:

| Archivo                            | Responsabilidad                                             |
| ---------------------------------- | ----------------------------------------------------------- |
| `main.py`                          | Punto de entrada, montaje de routers y middleware           |
| `config.py`                        | Lectura y validación de variables de entorno                |
| `database.py`                      | Pool de conexiones SQLAlchemy y context manager             |
| `schemas.py`                       | Modelos Pydantic de validación de respuestas                |
| `routers/health.py`                | Endpoint de salud                                           |
| `routers/players.py`               | Bloque 1: Tracker del jugador                               |
| `routers/heatmaps.py`              | Bloque 2: Heatmaps de mortalidad y navegación               |
| `routers/metrics.py`               | Bloque 2: Distribuciones agregadas y resúmenes              |
| `sql/schema_reference.sql`         | Contrato del esquema PostgreSQL esperado                    |
| `Dockerfile`                       | Imagen aislada con usuario no privilegiado y *healthcheck*  |

Cada router agrupa endpoints temáticamente relacionados y se monta dinámicamente desde `main.py` sobre el prefijo común `/api/v1`, manteniendo el endpoint de salud `/health` deliberadamente fuera del prefijo para que sea trivialmente accesible desde sondas externas.

##### **B. Configuración Centralizada (`config.py`)**

La gestión de variables de entorno se centraliza mediante `pydantic-settings`, evitando la dispersión de llamadas a `os.getenv()` por el código:

```python
class Settings(BaseSettings):
    postgres_url: str = Field(..., alias="POSTGRES_URL")
    db_pool_size: int = Field(5, alias="DB_POOL_SIZE")
    cors_origins: list[str] = Field(default=[...], alias="CORS_ORIGINS")

@lru_cache
def get_settings() -> Settings:
    return Settings()
```

El decorador `@lru_cache` garantiza que la instancia se construya una única vez durante el ciclo de vida del proceso, eliminando relecturas innecesarias. La validación tipada de Pydantic detecta errores de configuración en el arranque y no en tiempo de ejecución.

##### **C. Capa de Acceso Relacional (`database.py`)**

La conexión a PostgreSQL utiliza **SQLAlchemy Core** y deliberadamente prescinde del ORM completo. Esta elección responde a tres factores: rendimiento (no se mapean filas a clases Python), explicitud (cada consulta SQL es visible en el código) e inmunidad a inyección (parámetros nombrados separados del texto SQL).

```python
_engine = create_engine(
    settings.postgres_url,
    pool_size=5,
    max_overflow=10,
    pool_recycle=1800,
    pool_pre_ping=True,
    future=True,
)
```

Las dos opciones críticas son `pool_pre_ping=True`, que ejecuta un `SELECT 1` antes de entregar cada conexión para detectar las que estén muertas, y `pool_recycle=1800`, que descarta automáticamente conexiones con más de 30 minutos de antigüedad. Ambas resuelven el problema típico de las bases en la nube (Supabase, en este proyecto), donde el proveedor cierra silenciosamente las conexiones inactivas.

##### **D. Esquema Relacional Consultado (`sql/schema_reference.sql`)**

El esquema relacional sobre el que opera la Query API está diseñado para resolver dos problemas distintos con dos estrategias distintas. Por un lado, las consultas del **Tracker del jugador** requieren respuestas inmediatas con métricas ya calculadas (K/D, accuracy, TTL medio); por otro, las consultas de **Análisis** necesitan acceder a eventos individuales para construir distribuciones espaciales (heatmaps) y temporales (histogramas).

Esta dualidad se materializa en dos familias de tablas:

| Familia      | Tablas                                                    | Estrategia de escritura  | Volumen           |
| ------------ | --------------------------------------------------------- | ------------------------ | ----------------- |
| **Agregados**| `player_stats`, `session_stats`                           | UPSERT (idempotente)     | Pequeño y acotado |
| **Eventos**  | `death_events`, `position_events`, `item_events`          | INSERT (append-only)     | Crece linealmente |

###### **1. Tabla `player_stats` — Acumulado por Jugador**

Almacena una fila por jugador con sus estadísticas globales acumuladas a lo largo de toda su historia. Es la fuente directa del endpoint de perfil del Tracker.

```sql
CREATE TABLE IF NOT EXISTS player_stats (
    player_id              VARCHAR(64)   PRIMARY KEY,
    total_kills            INTEGER       NOT NULL DEFAULT 0,
    total_deaths           INTEGER       NOT NULL DEFAULT 0,
    kd_ratio               NUMERIC(10,4) NOT NULL DEFAULT 0,
    avg_accuracy           NUMERIC(5,4)  NOT NULL DEFAULT 0,
    avg_ttl_seconds        NUMERIC(8,2)  NOT NULL DEFAULT 0,
    total_sessions         INTEGER       NOT NULL DEFAULT 0,
    total_playtime_seconds INTEGER       NOT NULL DEFAULT 0,
    items_picked           INTEGER       NOT NULL DEFAULT 0,
    last_updated           TIMESTAMPTZ   NOT NULL DEFAULT NOW()
);
```

El worker actualiza esta tabla mediante `INSERT ... ON CONFLICT (player_id) DO UPDATE`, lo que garantiza que el resultado es el mismo independientemente del número de veces que se reprocese una misma sesión. El índice de la clave primaria es suficiente: todas las consultas filtran por `player_id`.

###### **2. Tabla `session_stats` — Resumen por Partida**

Almacena una fila por sesión jugada. Mantiene una referencia explícita a `player_stats` mediante clave foránea con `ON DELETE CASCADE`, de forma que borrar a un jugador limpia automáticamente todo su historial:

```sql
CREATE TABLE IF NOT EXISTS session_stats (
    session_id        VARCHAR(64)   PRIMARY KEY,
    player_id         VARCHAR(64)   NOT NULL
                      REFERENCES player_stats(player_id) ON DELETE CASCADE,
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

CREATE INDEX idx_session_player_time
    ON session_stats (player_id, started_at DESC);
```

El índice compuesto `(player_id, started_at DESC)` resulta crítico: los endpoints de historial y progresión filtran por jugador y ordenan por fecha, por lo que este índice convierte la consulta paginada en una operación de coste constante independiente del volumen total de la tabla.

###### **3. Tabla `death_events` — Registro Individual de Muertes**

Cada fila representa una muerte del jugador con sus coordenadas espaciales, su contexto y el TTL asociado (tiempo transcurrido desde el último `Player_Spawn`):

```sql
CREATE TABLE IF NOT EXISTS death_events (
    id            BIGSERIAL        PRIMARY KEY,
    session_id    VARCHAR(64)      NOT NULL,
    player_id     VARCHAR(64),
    pos_x         DOUBLE PRECISION NOT NULL,
    pos_z         DOUBLE PRECISION NOT NULL,
    floor_id      INTEGER,
    killer_id     VARCHAR(64),
    is_ai         BOOLEAN          NOT NULL DEFAULT FALSE,
    ttl_seconds   NUMERIC(8,2),
    occurred_at   TIMESTAMPTZ      NOT NULL
);

CREATE INDEX idx_death_floor   ON death_events (floor_id);
CREATE INDEX idx_death_session ON death_events (session_id);
CREATE INDEX idx_death_player  ON death_events (player_id);
```

Los tres índices secundarios responden a los tres filtros opcionales del endpoint de heatmap (`floor_id`, `session_id`, `player_id`), permitiendo segmentar la nube de calor sin escanear toda la tabla. La columna `ttl_seconds` alimenta el histograma de Time-to-Live; la columna `is_ai` permite distinguir muertes de jugadores reales de muertes de agentes IA.

###### **4. Tabla `position_events` — Heartbeats de Posición**

Almacena las trazas de movimiento del jugador, registradas cada cinco segundos mientras está vivo:

```sql
CREATE TABLE IF NOT EXISTS position_events (
    id           BIGSERIAL        PRIMARY KEY,
    session_id   VARCHAR(64)      NOT NULL,
    player_id    VARCHAR(64)      NOT NULL,
    pos_x        DOUBLE PRECISION NOT NULL,
    pos_z        DOUBLE PRECISION NOT NULL,
    floor_id     INTEGER,
    recorded_at  TIMESTAMPTZ      NOT NULL
);

CREATE INDEX idx_position_floor  ON position_events (floor_id);
CREATE INDEX idx_position_player ON position_events (player_id);
```

Esta es, por mucho, la tabla con mayor volumen de filas (una cada cinco segundos por jugador vivo). Por ello, el endpoint que la consulta impone un `LIMIT` agresivo por defecto (10 000 puntos), suficiente para construir heatmaps representativos sin saturar el navegador.

###### **5. Tabla `item_events` — Recogidas de Objetos**

Registra cada recogida individual con su tipo, permitiendo agregaciones por categoría desde el endpoint correspondiente:

```sql
CREATE TABLE IF NOT EXISTS item_events (
    id           BIGSERIAL    PRIMARY KEY,
    session_id   VARCHAR(64)  NOT NULL,
    player_id    VARCHAR(64)  NOT NULL,
    item_type    VARCHAR(32)  NOT NULL,
    occurred_at  TIMESTAMPTZ  NOT NULL
);

CREATE INDEX idx_item_player ON item_events (player_id);
CREATE INDEX idx_item_type   ON item_events (item_type);
```

El índice sobre `item_type` acelera la consulta `GROUP BY item_type` del endpoint de economía de objetos, evitando un *full scan* incluso cuando la tabla crece a cientos de miles de filas.

##### **E. Validación de Respuestas mediante Pydantic (`schemas.py`)**

Cada endpoint declara un `response_model` apuntando a una clase Pydantic definida en `schemas.py`. FastAPI utiliza estos modelos para tres propósitos simultáneos:

* **Validar** que la API devuelve exactamente lo que su contrato declara.
* **Generar** automáticamente el esquema OpenAPI 3.1 visible en `/docs` y `/redoc`.
* **Filtrar** campos accidentales antes de serializar el JSON de salida.

Los schemas se agrupan en tres bloques semánticos: **Salud** (`HealthStatus`), **Tracker del Jugador** (`PlayerProfile`, `SessionSummary`, `ProgressionPoint`) y **Análisis** (`HeatmapPoint`, `HeatmapResponse`, `TtlDistribution`, `ItemInteractionResponse`, `GlobalSummary`).

##### **F. Bloque 1 — Endpoints del Tracker del Jugador (`routers/players.py`)**

Este bloque alimenta el módulo de retención y competitividad del frontend. Expone tres endpoints diseñados para construir la vista personal del jugador. Todos parten de la tabla `player_stats` o `session_stats` y devuelven datos pre-calculados que el worker ya ha consolidado, lo que garantiza respuestas inmediatas.

###### **1. Perfil agregado — `GET /api/v1/players/{player_id}`**

Devuelve la tarjeta principal del Tracker leyendo directamente de `player_stats`. Recibe el identificador del jugador como parte de la ruta y no acepta parámetros adicionales.

**Ejemplo de petición:**
```http
GET /api/v1/players/mochomojao
```

**Ejemplo de respuesta (`200 OK`):**
```json
{
  "player_id": "mochomojao",
  "total_kills": 47,
  "total_deaths": 32,
  "kd_ratio": 1.4687,
  "avg_accuracy": 0.2143,
  "avg_ttl_seconds": 18.45,
  "total_sessions": 6,
  "total_playtime_seconds": 1247,
  "items_picked": 89
}
```

Si el identificador no existe, la API responde con `404 Not Found`:
```json
{ "detail": "Jugador 'desconocido' no encontrado" }
```

###### **2. Historial de sesiones — `GET /api/v1/players/{player_id}/sessions`**

Devuelve una lista paginada de las partidas jugadas, ordenadas de la más reciente a la más antigua. Acepta dos parámetros de paginación:

| Parámetro | Tipo | Default | Rango  | Descripción                          |
| --------- | ---- | ------- | ------ | ------------------------------------ |
| `limit`   | int  | `20`    | 1–200  | Número de sesiones por página        |
| `offset`  | int  | `0`     | ≥ 0    | Desplazamiento para paginación       |

**Ejemplo de petición:**
```http
GET /api/v1/players/mochomojao/sessions?limit=2&offset=0
```

**Ejemplo de respuesta:**
```json
[
  {
    "session_id": "d3b2a079-1f79-4025-a28a-8fbf6bbda060",
    "started_at": "2026-05-16T19:59:26+00:00",
    "ended_at":   "2026-05-16T20:01:26+00:00",
    "duration_seconds": 120,
    "kills": 4,
    "deaths": 10,
    "kd_ratio": 0.4,
    "accuracy": 0.2156,
    "avg_ttl_seconds": 17.32,
    "shots_fired": 87,
    "shots_hit": 19,
    "items_picked": 13
  },
  {
    "session_id": "f9a18c20-...",
    "started_at": "2026-05-15T18:14:02+00:00",
    "ended_at":   "2026-05-15T18:16:45+00:00",
    "duration_seconds": 163,
    "kills": 7,
    "deaths": 8,
    "kd_ratio": 0.875,
    "accuracy": 0.2381,
    "avg_ttl_seconds": 19.48,
    "shots_fired": 105,
    "shots_hit": 25,
    "items_picked": 16
  }
]
```

###### **3. Curva de progresión — `GET /api/v1/players/{player_id}/progression`**

Endpoint diseñado específicamente para validar la **Hipótesis 3**. Devuelve las últimas N sesiones del jugador en **orden cronológico ascendente** (la más antigua primero), junto con un número de partida secuencial calculado en SQL:

```sql
ROW_NUMBER() OVER (ORDER BY started_at ASC) AS session_number
```

| Parámetro | Tipo | Default | Rango  | Descripción                          |
| --------- | ---- | ------- | ------ | ------------------------------------ |
| `limit`   | int  | `50`    | 1–500  | Número de puntos de la curva         |

**Ejemplo de petición:**
```http
GET /api/v1/players/mochomojao/progression?limit=3
```

**Ejemplo de respuesta:**
```json
[
  { "session_number": 1, "session_id": "a1...", "played_at": "2026-05-10T17:01:00+00:00", "kd_ratio": 0.20, "accuracy": 0.1421, "avg_ttl_seconds": 12.30 },
  { "session_number": 2, "session_id": "b2...", "played_at": "2026-05-12T18:45:00+00:00", "kd_ratio": 0.45, "accuracy": 0.1843, "avg_ttl_seconds": 15.10 },
  { "session_number": 3, "session_id": "c3...", "played_at": "2026-05-14T20:22:00+00:00", "kd_ratio": 0.85, "accuracy": 0.2156, "avg_ttl_seconds": 18.00 }
]
```

Esta numeración permite al frontend renderizar la lista directamente sobre un gráfico de líneas de Recharts. Si la pendiente del ajuste lineal sobre `kd_ratio` y `accuracy` es positiva a lo largo de las sesiones, H3 queda empíricamente corroborada.

##### **G. Bloque 2 — Endpoints de Análisis e Investigación**

Esta familia de endpoints alimenta el Laboratorio de Investigación interno y proporciona los datos cuantitativos necesarios para validar las hipótesis H1, H2 y H4. A diferencia del Bloque 1, estas consultas operan sobre las tablas de eventos crudos (`death_events`, `position_events`, `item_events`) y aplican agregaciones SQL en tiempo de petición.

###### **1. Heatmap de mortalidad — `GET /api/v1/heatmaps/deaths`**

Sirve coordenadas `(x, z)` de muertes registradas en formato directamente compatible con la librería `simpleheat` del frontend. Implementa un constructor seguro de cláusulas `WHERE` para combinar filtros opcionales sin riesgo de inyección:

```python
conditions: list[str] = ["TRUE"]
params: dict[str, object] = {}

if floor_id is not None:
    conditions.append("floor_id = :floor_id")
    params["floor_id"] = floor_id
```

| Parámetro            | Tipo   | Default | Rango     | Descripción                                |
| -------------------- | ------ | ------- | --------- | ------------------------------------------ |
| `floor_id`           | int    | —       | —         | Filtra por planta del mapa                 |
| `session_id`         | string | —       | —         | Limita a una partida concreta              |
| `player_id`          | string | —       | —         | Limita a las muertes de un jugador         |
| `include_ai_deaths`  | bool   | `true`  | —         | Si `false`, solo muertes humanas           |
| `limit`              | int    | `5000`  | 1–50000   | Tope máximo de puntos                      |

**Ejemplo de petición:**
```http
GET /api/v1/heatmaps/deaths?floor_id=1&include_ai_deaths=false&limit=500
```

**Ejemplo de respuesta:**
```json
{
  "floor_id": 1,
  "point_count": 3,
  "points": [
    { "x": -28.99, "z":  -6.52, "value": 1.0 },
    { "x": -17.01, "z":  -7.21, "value": 1.0 },
    { "x":  22.11, "z": -24.35, "value": 1.0 }
  ]
}
```

El resultado es la métrica espacial M2.2 que permite identificar los choke points donde se concentran las bajas (Hipótesis 2).

###### **2. Heatmap de navegación — `GET /api/v1/heatmaps/navigation`**

Análogo al anterior pero sobre los heartbeats de posición registrados cada cinco segundos mientras el jugador está vivo. Constituye la métrica M2.1 y revela las zonas más transitadas del mapa.

| Parámetro    | Tipo   | Default  | Rango      | Descripción                          |
| ------------ | ------ | -------- | ---------- | ------------------------------------ |
| `floor_id`   | int    | —        | —          | Filtra por planta del mapa           |
| `session_id` | string | —        | —          | Limita a una partida concreta        |
| `player_id`  | string | —        | —          | Limita a un jugador                  |
| `limit`      | int    | `10000`  | 1–100000   | Tope máximo de puntos                |

**Ejemplo de petición:**
```http
GET /api/v1/heatmaps/navigation?player_id=mochomojao&limit=200
```

**Ejemplo de respuesta:** mismo formato que el heatmap de mortalidad, con coordenadas `(x, z)` de los puntos por los que ha pasado el jugador. La comparación visual entre ambos heatmaps (mortalidad frente a tránsito) permite distinguir las zonas seguras de las zonas calientes, validando o refutando la fricción navegacional descrita en la Hipótesis 2.

###### **3. Distribución del Time-to-Live — `GET /api/v1/metrics/ttl-distribution`**

Construye el histograma del tiempo de supervivencia agrupando los TTL almacenados en `death_events` por buckets configurables:

```sql
SELECT
    FLOOR(ttl_seconds / :bucket) * :bucket AS bucket_start,
    COUNT(*) AS death_count
FROM death_events
WHERE ttl_seconds IS NOT NULL
GROUP BY bucket_start
ORDER BY bucket_start ASC
```

| Parámetro              | Tipo   | Default | Rango | Descripción                              |
| ---------------------- | ------ | ------- | ----- | ---------------------------------------- |
| `player_id`            | string | —       | —     | Restringe el histograma a un jugador     |
| `bucket_size_seconds`  | int    | `5`     | 1–60  | Anchura de cada bucket en segundos       |

**Ejemplo de petición:**
```http
GET /api/v1/metrics/ttl-distribution?bucket_size_seconds=5
```

**Ejemplo de respuesta:**
```json
{
  "bucket_size_seconds": 5,
  "total_deaths": 42,
  "mean_seconds": 12.18,
  "median_seconds": 9.30,
  "buckets": [
    { "bucket_start_seconds":  0.0, "bucket_end_seconds":  5.0, "count": 14 },
    { "bucket_start_seconds":  5.0, "bucket_end_seconds": 10.0, "count": 13 },
    { "bucket_start_seconds": 10.0, "bucket_end_seconds": 15.0, "count":  8 },
    { "bucket_start_seconds": 15.0, "bucket_end_seconds": 20.0, "count":  4 },
    { "bucket_start_seconds": 20.0, "bucket_end_seconds": 25.0, "count":  3 }
  ]
}
```

La mediana se calcula con la función nativa de PostgreSQL `PERCENTILE_CONT(0.5) WITHIN GROUP (ORDER BY ttl_seconds)`, garantizando precisión estadística. Si la masa del histograma se concentra en los primeros buckets (0–10 segundos) y la mediana es baja, la **Hipótesis 1** (mortalidad temprana desproporcionada) queda respaldada empíricamente.

###### **4. Economía de objetos — `GET /api/v1/metrics/item-interactions`**

Cuenta las recogidas agrupadas por tipo de objeto. Permite responder cuantitativamente a la pregunta planteada por la **Hipótesis 4**: si los jugadores ignoran sistemáticamente los recursos secundarios y se mantienen con el arma inicial, la desproporción entre categorías será evidente.

| Parámetro    | Tipo   | Default | Descripción                              |
| ------------ | ------ | ------- | ---------------------------------------- |
| `player_id`  | string | —       | Limita el cálculo a un jugador concreto  |

**Ejemplo de petición:**
```http
GET /api/v1/metrics/item-interactions
```

**Ejemplo de respuesta:**
```json
{
  "total_pickups": 142,
  "by_item_type": [
    { "item_type": "Rifle",  "pickup_count": 58 },
    { "item_type": "Pistol", "pickup_count": 41 },
    { "item_type": "Rocket", "pickup_count": 27 },
    { "item_type": "Health", "pickup_count": 12 },
    { "item_type": "Ammo",   "pickup_count":  4 }
  ]
}
```

Los resultados se ordenan por frecuencia descendente, facilitando que el frontend los pinte directamente sobre un gráfico circular sin necesidad de reordenación.

###### **5. Resumen global — `GET /api/v1/metrics/global-summary`**

Devuelve los KPIs agregados del playtest. Es el endpoint más ligero del sistema: una única consulta SQL con cuatro subconsultas y un agregado. No acepta parámetros.

**Ejemplo de petición:**
```http
GET /api/v1/metrics/global-summary
```

**Ejemplo de respuesta:**
```json
{
  "total_players": 5,
  "total_sessions": 31,
  "total_deaths": 287,
  "global_avg_kd": 0.8421,
  "global_avg_accuracy": 0.2089,
  "global_avg_ttl_seconds": 14.62
}
```

Constituye la cabecera natural del dashboard interno, equivalente a la fila de indicadores que suele aparecer en la parte superior de cualquier panel analítico.

##### **H. Endpoint de Salud (`routers/health.py`)**

El endpoint `GET /health` queda deliberadamente fuera del prefijo `/api/v1` para facilitar el acceso desde el `HEALTHCHECK` del contenedor Docker y desde sondas externas de monitorización. No se limita a comprobar que el servidor responde: ejecuta un `SELECT 1` real contra PostgreSQL para verificar la conectividad efectiva con la base de datos.

**Ejemplo de respuesta (`200 OK`):**
```json
{
  "status": "ok",
  "database": "up",
  "version": "1.0.0",
  "timestamp": "2026-05-18T10:32:14.215843+00:00"
}
```

**Ejemplo de respuesta (`503 Service Unavailable`):**
```json
{
  "status": "degraded",
  "database": "down",
  "version": "1.0.0",
  "timestamp": "2026-05-18T10:32:14.215843+00:00"
}
```

El código HTTP varía en función del estado real del sistema, permitiendo que Docker marque automáticamente el contenedor como `unhealthy` y dispare las políticas de reinicio configuradas.

##### **I. Manejo Centralizado de Errores**

Dos manejadores globales registrados en `main.py` convierten las excepciones más frecuentes en respuestas JSON consistentes:

```python
@app.exception_handler(SQLAlchemyError)
async def db_error_handler(request, exc):
    logger.exception("Error de BD en %s %s", ...)
    return JSONResponse(
        status_code=503,
        content={"detail": "Error al consultar la base de datos"},
    )
```

* `SQLAlchemyError` → `503 Service Unavailable` con un mensaje genérico. El *stacktrace* se registra en los logs internos pero **nunca se expone al cliente**, evitando filtración de información sensible sobre la estructura interna.
* `RequestValidationError` → `422 Unprocessable Entity` con el detalle de los parámetros inválidos, facilitando la depuración del frontend.

##### **J. Política CORS para Integración con el Frontend**

Sin una política CORS explícita, el navegador bloquearía las peticiones del frontend React por incumplir la política de mismo origen. El middleware `CORSMiddleware` se configura para permitir únicamente lo estrictamente necesario:

```python
app.add_middleware(
    CORSMiddleware,
    allow_origins=get_settings().cors_origins,
    allow_credentials=False,
    allow_methods=["GET"],
    allow_headers=["*"],
)
```

La política se restringe al método `GET` (coherente con el carácter de solo lectura del servicio) y a los orígenes declarados en `CORS_ORIGINS`, que por defecto incluyen los puertos típicos de desarrollo (`localhost:3000` para CRA y `localhost:5173` para Vite).

##### **K. Documentación Interactiva Automática**

FastAPI genera automáticamente dos interfaces de documentación a partir de los `response_model` y las descripciones declarativas de los endpoints:

| Ruta      | Interfaz                  | Función                                          |
| --------- | ------------------------- | ------------------------------------------------ |
| `/docs`   | **Swagger UI**            | Pruebas interactivas con botón *Execute*         |
| `/redoc`  | **ReDoc**                 | Documentación legible en formato de lectura      |

Ambas se actualizan sin intervención manual cada vez que se modifica un schema o se añade un endpoint, eliminando la deuda de mantenimiento típica de la documentación escrita a mano y garantizando que la documentación nunca quede desincronizada del código real.

##### **L. Empaquetado y Despliegue con Docker**

La imagen se construye sobre `python:3.11-slim` aplicando varias buenas prácticas de seguridad y eficiencia:

```Dockerfile
FROM python:3.11-slim
ENV PYTHONDONTWRITEBYTECODE=1 PYTHONUNBUFFERED=1
WORKDIR /app
COPY requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt
COPY . .
RUN useradd --create-home queryapi && chown -R queryapi:queryapi /app
USER queryapi
HEALTHCHECK CMD python -c "import urllib.request; ..."
CMD ["uvicorn", "main:app", "--host", "0.0.0.0", "--port", "8001"]
```

* **Capa de dependencias separada del código**: copia primero `requirements.txt`, instala y solo después copia el resto. Esto permite a Docker reutilizar la capa cacheada en cada rebuild, acelerando drásticamente los ciclos de desarrollo.
* **Usuario no privilegiado (`queryapi`)**: el contenedor se ejecuta bajo un usuario sin permisos de *root*, reduciendo la superficie de ataque ante una eventual vulnerabilidad.
* **`HEALTHCHECK` nativo**: Docker comprueba cada 30 segundos el endpoint `/health` y marca el contenedor como `unhealthy` tras tres fallos consecutivos.
* **`PYTHONUNBUFFERED=1`**: garantiza que los logs salgan en tiempo real a `docker compose logs`, sin quedar atrapados en buffer.

El servicio se orquesta desde el `docker-compose.yml` raíz del proyecto y depende implícitamente de la variable `POSTGRES_URL` definida en el `.env` compartido. Si la conexión a PostgreSQL falla durante el arranque, el *lifespan* de FastAPI lo detecta inmediatamente y termina el contenedor con error, evitando un servicio "vivo pero inservible".

En conjunto, esta capa transforma la base relacional inerte producida por el worker en una API REST viva, segura, autodocumentada y lista para integrarse tanto con interfaces de usuario finales como con herramientas internas de análisis cuantitativo.

--- 

#### **3.5. Implementación de la Capa de Visualización y Análisis Web (Frontend)**

La capa final de la arquitectura la constituye la aplicación web estática (SPA), un centro de mando analítico diseñado para interactuar de forma interactiva con los datos procesados en la infraestructura relacional.

##### **A. Stack Tecnológico y Justificación de Herramientas**
Para el desarrollo de la interfaz de usuario se descartaron frameworks tradicionales pesados en favor de un entorno ágil, moderno y de alto rendimiento adaptado a la estética de la industria del videojuego:

* **React (v19) + Vite (v8):** Se seleccionó React por su modelo de renderizado reactivo basado en componentes y estados dinámicos, ideal para paneles interactivos. **Vite** se implementó como empaquetador y motor de desarrollo debido a su arranque instantáneo mediante módulos ESM nativos y su *Hot Module Replacement* (HMR), reduciendo los tiempos de compilación frente a herramientas clásicas como Webpack.
* **Tailwind CSS (v4):** La interfaz adopta un diseño oscuro de corte cibernético/gaming (`bg-slate-900`, acentos neón `text-cyan-400` y `text-purple-500`). Tailwind CSS permitió maquetar este estilo directamente sobre las clases de los componentes HTML sin generar hojas de estilo externas complejas, optimizando el rendimiento visual y la velocidad de desarrollo.
* **React Router (v7):** Configura la navegación interna del sitio web como una *Single Page Application* (SPA). Al cambiar de pestaña, el enrutador recompone el DOM dinámicamente en el cliente sin realizar peticiones de recarga al navegador, garantizando una fluidez de navegación instantánea.
* **Recharts (v3):** Librería gráfica nativa de React para la visualización estadística. Es fundamental para mapear las métricas de rendimiento ofensivo y evaluar de forma interactiva la evolución temporal del jugador.
* **Simpleheat (Resolución del Reto de los Heatmaps):** se escogió **`simpleheat`**, una librería moderna y ligera creada por el equipo de Mapbox que dibuja nubes de densidad térmica directamente sobre un objeto `<canvas>` nativo de HTML5, respetando los estándares modernos de rendimiento y aislamiento de memoria.

##### **B. Estructura Modular del Código Fuente**
El proyecto web se organiza de forma modular y desacoplada dentro del directorio `/frontend/src` para separar de forma nítida la UI de la lógica de red:

* `services/api.js` **(Capa de Aislamiento de Red):** Centraliza las peticiones asíncronas hacia la *Query API* a través de la API `fetch` nativa. Utiliza la variable de entorno `import.meta.env.VITE_API_URL` inyectada por Docker. Esta capa implementa un formateador basado en `URLSearchParams` para limpiar las consultas con filtros y centraliza el manejo de excepciones (HTTP 404 de jugador inexistente o 503 de caída de base de datos) mediante una función auxiliar `fetchWithError`, protegiendo a la interfaz de fallos no controlados.
* `components/Layout.jsx` **(Marco de UI Persistente):** Define la estructura fija de la aplicación. Renderiza la barra de navegación superior (Navbar) equipada con enlaces dinámicos que se iluminan según la ruta activa detectada por el hook `useLocation`. Utiliza el componente `<Outlet />` de React Router para inyectar dinámicamente las páginas solicitadas dentro del contenedor principal sin reconstruir el menú de navegación.
* `pages/Home.jsx` **(El Hub Central):** Actúa como puerta de acceso para los usuarios. Cuenta con un buscador que redirige de forma segura a la ruta dinámica del perfil de telemetría personal. Además, implementa un efecto de montado (`useEffect`) que interroga al endpoint `/metrics/global-summary` de la API para desplegar en tiempo real el estado del clúster y el volumen de partidas almacenadas.
* `pages/PlayerTracker.jsx` **(Módulo de Retención y Competitividad):** Implementa el "Bloque 1" del plan analítico. Muestra el rendimiento del jugador procesado en PostgreSQL.
* `pages/LabDashboard.jsx` **(Módulo de Investigación para Game Designers):** Implementa el "Bloque 2". Cruza métricas económicas globales mediante gráficos circulares y renderiza los mapas térmicos espaciales para el estudio del Level Design.

##### **C. Implementación de Procesos Clave y Fragmentos de Código**

###### **1. Sincronización Asíncrona en Paralelo (`Promise.all`)**
Para evitar la carga secuencial bloqueante, la pantalla del perfil del jugador unifica las llamadas a los microservicios relacionales ejecutándolas en paralelo. De este modo, la interfaz reduce el tiempo de espera al equivalente de la llamada más lenta:

```jsx
// Fragmento de control de flujo asíncrono en PlayerTracker.jsx
useEffect(() => {
  setLoading(true);
  setError(null);

  // Ejecución simultánea de las consultas del Perfil, Curva de Progresión e Historial
  Promise.all([
    api.getPlayerProfile(id),
    api.getPlayerProgression(id),
    api.getPlayerSessions(id, 10, 0) // Trae el subset de las últimas 10 sesiones
  ])
  .then(([profileData, progressionData, sessionsData]) => {
    setProfile(profileData);
    setProgression(progressionData);
    setSessions(sessionsData);
    setLoading(false);
  })
  .catch((err) => {
    console.error(err);
    setError("No se pudieron recuperar los registros del jugador. Verifique el ID único.");
    setLoading(false);
  });
}, [id]);

```

###### **2. Configuración de Gráficos Duales para Variables Heterogéneas**

Para evaluar la **Hipótesis 3 (H3)**, se requiere correlacionar la evolución cronológica del ratio K/D (número entero abierto, M3.1) con la tasa de precisión (porcentaje cerrado entre 0 y 1, M3.2). Dado que sus escalas numéricas son incompatibles, se implementó en Recharts una configuración de **eje Y dual independiente** (`yAxisId="left"` y `yAxisId="right"`), permitiendo proyectar ambas tendencias en un único plano cartesiano legible para el usuario:

```jsx
// Configuración de la composición del gráfico en Recharts
<LineChart data={progression}>
  <CartesianGrid strokeDasharray="3 3" stroke="#1e293b" />
  <XAxis dataKey="session_number" stroke="#64748b" tickFormatter={(v) => `Partida ${v}`} />
  <YAxis yAxisId="left" stroke="#f43f5e" /> {/* Eje ofensivo para K/D */}
  <YAxis yAxisId="right" orientation="right" stroke="#06b6d4" tickFormatter={(v) => `${(v * 100).toFixed(0)}%`} /> {/* Eje de habilidad */}
  <Tooltip contentStyle={{ backgroundColor: '#0f172a', borderColor: '#334155' }} />
  <Line yAxisId="left" type="monotone" dataKey="kd_ratio" name="Ratio K/D" stroke="#f43f5e" strokeWidth={3} />
  <Line yAxisId="right" type="monotone" dataKey="accuracy" name="Precisión %" stroke="#06b6d4" strokeWidth={2} />
</LineChart>

```

###### **3. El Desafío Espacial: Conversión Matemática de Coordenadas Unity -> Web Canvas**

El reto más complejo del frontend radicó en la incompatibilidad tridimensional. Unity registra la posición del avatar en un plano cartesiano flotante `X/Z` donde el origen `(0,0)` suele situarse en el baricentro de la escena física y el eje `Z+` apunta hacia el Norte. Por el contrario, la API de renderizado del navegador gestiona una matriz de píxeles estricta en 2D `X/Y`, donde el punto `(0,0)` se ancla rígidamente en la esquina superior izquierda del viewport y el eje `Y+` desciende verticalmente hacia el suelo de la pantalla.

Para superponer con exactitud milimétrica los clústeres de mortalidad sobre la captura cenital estática del nivel (`/src/assets/mapa_base.png`), se diseñó un algoritmo de normalización lineal que calcula un factor de escala proporcional y corrige la inversión del eje de ordenadas:

$$\text{scaleX} = \frac{\text{canvas.width}}{X_{\text{max}} - X_{\text{min}}}$$

$$\text{scaleZ} = \frac{\text{canvas.height}}{Z_{\text{max}} - Z_{\text{min}}}$$

$$X_{\text{web}} = (X_{\text{unity}} - X_{\text{min}}) \times \text{scaleX}$$

$$Y_{\text{web}} = \text{canvas.height} - \left[(Z_{\text{unity}} - Z_{\text{min}}) \times \text{scaleZ}\right]$$

La implementación exacta de esta traslación espacial y su posterior inyección segura en la instancia de `simpleheat` quedó codificada en el dashboard analítico de la siguiente manera:

```jsx
// Bloque de transformación geométrica y pintado térmico en LabDashboard.jsx
useEffect(() => {
  if (!loading && mapContainerRef.current && heatmapPoints.length > 0) {
    const canvas = mapContainerRef.current;
    
    // Sincronización explícita de la resolución nativa interna con las dimensiones CSS en pantalla
    canvas.width = canvas.clientWidth;
    canvas.height = canvas.clientHeight;

    // Inicialización del motor gráfico bidimensional de simpleheat
    const heat = simpleheat(canvas);
    heat.radius(25, 15); // Configuración del radio del foco térmico y su desenfoque (blur)

    // Cálculo dinámico de factores de proporción según los límites reales del mapa en Unity
    const unityWidth = UNITY_MAX_X - UNITY_MIN_X;
    const unityHeight = UNITY_MAX_Z - UNITY_MIN_Z;
    const scaleX = canvas.width / unityWidth;
    const scaleZ = canvas.height / unityHeight;

    // Mapeo adaptativo y conversión matricial de los eventos de muerte
    const dataPoints = heatmapPoints.map(point => {
      let xWeb = (point.x - UNITY_MIN_X) * scaleX;
      let yWeb = canvas.height - ((point.z - UNITY_MIN_Z) * scaleZ); // Inversión del eje Z para alinear el Norte

      return [
        Math.floor(xWeb), 
        Math.floor(yWeb), 
        point.value || 1 // Intensidad o peso del evento de muerte
      ];
    });

    // Inyección de datos filtrados y renderizado final sobre el lienzo de dibujo
    heat.data(dataPoints);
    heat.max(5); // Umbral de saturación: 5 bajas concentradas saturan el gradiente a Rojo Intenso
    heat.draw();
  }
}, [loading, heatmapPoints]);

```

##### **D. Automatización y Despliegue con Docker (Explicación del Entorno Visual)**

Para garantizar que la página web funcione exactamente igual en el ordenador de cualquier usuario o evaluador, todo el sistema del frontend se ha empaquetado utilizando contenedores de **Docker**. A continuación, se explica de forma accesible cómo funciona esta integración:

1. **Entorno Limpio e Independiente (Dockerfile):** Se configuró un "contenedor" (un entorno virtual aislado e independiente) que simula un miniordenador preconfigurado con Node.js. Al arrancar, este entorno descarga automáticamente todas las librerías necesarias de forma limpia y ejecuta el motor de la web (Vite). Finalmente, abre un canal de comunicación exclusivo (el puerto `5173`) para que podamos acceder a la interfaz desde cualquier navegador web usando `http://localhost:5173`.
2. **Protección contra Conflictos del Sistema (Aislamiento de Librerías):** El ordenador físico desde el que trabajamos y el contenedor de Docker organizan sus archivos de forma interna muy diferente. Si las librerías de código de ambos sistemas se mezclaran directamente, la aplicación web se corrompería y dejaría de funcionar. Para evitar esto, se creó una "caja fuerte" de memoria protegida dentro de Docker exclusivamente para almacenar las librerías del proyecto (`node_modules`), garantizando la estabilidad y compatibilidad absoluta del sistema.
3. **Actualización del Código en Vivo (Sincronización en Tiempo Real):** Se estableció un puente que conecta los archivos de nuestro ordenador con el contenedor de Docker. Gracias a esta conexión, cualquier cambio que realicemos en el diseño visual o en la lógica de las gráficas desde nuestro editor de texto se propaga al instante. La web del navegador se actualiza automáticamente en milisegundos sin necesidad de apagar, reconstruir o reiniciar los servidores, optimizando drásticamente la velocidad en las fases de prueba.

```
