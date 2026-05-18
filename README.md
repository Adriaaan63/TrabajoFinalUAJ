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
