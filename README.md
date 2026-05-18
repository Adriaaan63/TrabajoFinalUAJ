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

* **1. Implementación del sistema de captura de eventos en el cliente Unity**

integrando el *Tracker* de telemetría con el framework Opsive Deathmatch AI Kit mediante el sistema de eventos nativos del motor.

**Definición del diccionario de eventos (`GameplayEvents.cs`)**

Se diseñaron e implementaron todas las clases de evento de gameplay del proyecto, cada una extendiendo la clase base `TrackerEvent` del sistema de telemetría. El diccionario cubre los siete eventos acordados en la especificación: `Player_Spawn`, `Player_Death`, `AI_Death`, `Shot_Fired`, `Shot_Hit`, `Player_Position_Heartbeat` e `Item_Picked`. Cada clase encapsula exclusivamente los atributos relevantes para su tipo (posición, identificador del killer, zona de impacto, tipo de ítem o nombre del arma), manteniendo los payloads compactos y coherentes con el esquema esperado por la API de ingesta.

**Captura de eventos del jugador (`PlayerEventHooks.cs`)**

Se implementó el script `PlayerEventHooks`, que se adjunta al prefab del jugador y se suscribe a tres eventos del `EventHandler` de Opsive: `OnDeath` para registrar `Player_Death` con posición y killer, `OnRespawn` para registrar `Player_Spawn` con las coordenadas X/Z de reaparición, y `OnItemUseComplete` para registrar `Shot_Fired` con el nombre del arma disparada. Adicionalmente, el script ejecuta una corutina interna que emite un evento `Player_Position_Heartbeat` cada cinco segundos con la posición actual del personaje, permitiendo reconstruir el recorrido del jugador a lo largo de la sesión.

**Captura de eventos de agentes IA (`AIEventHooks.cs`)**

Se implementó el script `AIEventHooks`, que se adjunta al prefab de cada agente enemigo. Se suscribe a `OnDeath` para registrar `AI_Death` con posición y killer, y a `OnHealthDamage` para detectar impactos recibidos. En este último caso, el handler filtra por el tag `Player` en el parámetro `attacker` antes de emitir `Shot_Hit`, evitando registrar daño entre agentes IA. La zona de impacto se infiere del nombre del `Collider` afectado, clasificando el golpe como `Head` o `Body`.

**Captura de recogida de ítems (`ItemPickupHooks.cs`)**

Se implementó el script `ItemPickupHooks`, que se adjunta a cada ítem del mapa y expone un campo configurable `itemType` en el Inspector de Unity. El script se suscribe al evento `OnItemPickup` del propio GameObject del ítem y registra `Item_Picked` únicamente cuando el parámetro `picker` corresponde a un objeto con el tag `Player`, descartando así cualquier interacción de la IA con los objetos del escenario.

**Resolución de incompatibilidades con el framework de Opsive**

Durante la integración se identificaron y resolvieron varias incompatibilidades entre la documentación del kit y su versión instalada. El `EventHandler` se encontraba bajo el namespace `Opsive.Shared.Events` y no bajo `Opsive.UltimateCharacterController.Events`. El evento `OnDeath` requería una firma `InvokableAction<Vector3, Vector3, GameObject>` en lugar de una `Action` simple, causando excepción en tiempo de ejecución con la firma incorrecta. El evento de daño recibido se denominaba `OnHealthDamage` en lugar de `OnHealthDamageReceived`. Finalmente, el evento de disparo `OnItemUseComplete` esperaba un parámetro de tipo `IUsableItem` en lugar de `System.Object`. Cada incompatibilidad se diagnosticó mediante scripts de detección temporal que registraban en consola los eventos y tipos reales lanzados por el framework, permitiendo corregir las firmas sin modificar código interno de Opsive.

* **2. Implementación de la persistencia de Unity a Docker**

Implementación del subsistema de persistencia desde Unity hacia la infraestructura Docker, así como en la definición del esqueleto de la base de datos y el motor de procesamiento de métricas.

**Capa de persistencia en Unity (`DockerPersistence.cs`)**

Se diseñó e implementó la clase `DockerPersistence`, una nueva implementación de la interfaz `IPersistence` que permite al tracker de Unity enviar los datos de sesión directamente a la API de ingesta dockerizada. La clase adopta una estrategia de acumulación en memoria mediante un `StringBuilder` durante toda la partida, posponiendo el envío HTTP hasta el cierre limpio de la aplicación (`OnApplicationQuit`). Esto garantiza que el payload enviado contiene la sesión completa y es coherente con el endpoint `/upload_session` de la API. El envío se realiza de forma síncrona con `HttpClient`, siguiendo el mismo patrón que la persistencia de Firebase ya existente en el proyecto.

**Integración en el sistema de configuración (`TrackerConfig.cs` y `TrackerInitializer.cs`)**

Se extendió `TrackerConfig` con el nuevo campo `dockerApiUrl` para permitir configurar la URL del contenedor tanto desde el Inspector de Unity como desde el fichero externo `tracker.config.json`, sin necesidad de recompilar. En `TrackerInitializer` se añadió la opción `DOCKER` al enumerado `P_type`, el bloque de interfaz visual en el Inspector correspondiente y la lógica de instanciación en `ChoosePersistence()`, incluyendo un fallback automático a `LocalFile` si la URL está vacía. De esta forma la nueva persistencia queda completamente integrada en el ciclo de vida del tracker existente sin romper ninguna funcionalidad previa.

**API de ingesta (`ingest_api/main.py`) [Junto a Marcos Pérez]**

Se implementó el endpoint `POST /upload_session`, que era inexistente en el código original. El endpoint valida la presencia de los campos obligatorios `session_id` y `events`, añade metadatos de servidor (`received_at`, `processed: false`), persiste el documento completo en MongoDB Atlas como respaldo permanente, y encola el identificador de Mongo en Redis mediante `rpush` para notificar al worker de forma asíncrona.

**Motor de procesamiento de métricas (`metrics_worker/worker.py`) [Junto a Marcos Pérez]**

Se reescribió el worker desde el esqueleto vacío original hasta una implementación funcional completa. El worker escucha la cola Redis mediante `blpop` bloqueante, recupera la sesión de MongoDB por su `_id`, calcula las métricas de juego filtrando eventos por tipo y ejecuta un UPSERT en PostgreSQL que acumula estadísticas entre sesiones del mismo jugador con una media de precisión ponderada por número de partidas.
