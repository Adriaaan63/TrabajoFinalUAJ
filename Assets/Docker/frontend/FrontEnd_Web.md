# Frontend Web - Opsive FPS Telemetry Tracker

Este documento detalla la arquitectura, las decisiones tecnológicas y el flujo de trabajo utilizado para construir la interfaz web del sistema de telemetría. Este frontend actúa como la capa final de visualización, consumiendo los datos procesados en PostgreSQL a través de la Query API.

---

## 🛠️ 1. Stack Tecnológico y Justificación

Para construir un panel de control moderno, rápido y orientado a videojuegos, abandonamos las plantillas tradicionales y optamos por el siguiente stack:

* **React + Vite:** * *¿Por qué?* React es el estándar de la industria para interfaces basadas en componentes y estados dinámicos. Elegimos **Vite** en lugar del clásico *Create React App* porque su servidor de desarrollo es instantáneo y la compilación es infinitamente más rápida gracias a su empaquetador nativo.
* **Tailwind CSS:** * *¿Por qué?* En lugar de escribir largos archivos CSS separados, Tailwind permite diseñar directamente en el HTML usando "clases de utilidad". Esto nos permitió iterar un diseño estilo *Gaming* (fondos oscuros `bg-slate-900`, acentos neón `text-cyan-400`) a una velocidad asombrosa.
* **React Router (`react-router-dom`):** * *¿Por qué?* Permite tener una SPA (*Single Page Application*). El usuario puede navegar del Hub al Perfil del Jugador sin que el navegador tenga que recargar la página completa, dando una sensación de aplicación nativa.
* **Recharts:** * *¿Por qué?* Una librería de gráficos construida específicamente para React. Nos permite validar visualmente la **Hipótesis 3** (curva de aprendizaje) combinando ratios (enteros) y precisiones (porcentajes) en ejes duales sin complicaciones.
* **Lucide React:** * *¿Por qué?* Una librería de iconos SVG limpios, modernos y ligeros (`<Skull />`, `<Target />`, `<Crosshair />`).
* **Simpleheat (El Reto de los Heatmaps):** * *La decisión:* Inicialmente el plan indicaba usar `heatmap.js`. Sin embargo, al implementarlo descubrimos un error crítico: `Cannot assign to read only property 'data' of object '#<ImageData>'`. Esto ocurre porque `heatmap.js` lleva años sin actualizarse y los navegadores modernos bloquean la sobrescritura directa de memoria en los canvas por seguridad. 
    * *La solución:* Migramos a **`simpleheat`**, el motor subyacente creado por Mapbox. Es una alternativa hiperligera que dibuja sobre una etiqueta `<canvas>` nativa respetando los estándares de seguridad modernos.

---

## 📂 2. Estructura de Archivos y Explicación

El código está organizado modularmente dentro de `/frontend/src`:

```text
/src
├── /components
│   └── Layout.jsx       # (UI Persistente)
├── /pages
│   ├── Home.jsx         # (Ruta: "/")
│   ├── PlayerTracker.jsx# (Ruta: "/player/:id")
│   └── LabDashboard.jsx # (Ruta: "/lab")
├── /services
│   └── api.js           # (Capa de Red)
├── App.jsx              # (Enrutador Principal)
├── main.jsx             # (Punto de Entrada)
└── index.css            # (Configuración Global)

```

### Explicación de los Archivos Clave

#### `services/api.js` (El Puente)

Centraliza todas las peticiones `fetch` a la Query API.

* **¿Por qué?** Si la URL del servidor cambia, o si añadimos tokens de seguridad, solo modificamos este archivo. Los componentes visuales (como React) no deben saber de dónde vienen los datos, solo pedirlos. Usamos `URLSearchParams` para evitar errores al construir URLs con filtros.

#### `components/Layout.jsx`

Es el "marco" de la aplicación. Contiene la barra de navegación superior (Navbar).

* **Uso:** Envuelve a todas las páginas mediante el componente `<Outlet />` de React Router. Así, el menú siempre está visible sin recargarse.

#### `pages/Home.jsx` (El Hub)

La puerta de entrada. Contiene una barra de búsqueda que redirige al usuario a su perfil y hace una llamada rápida a `/metrics/global-summary` para confirmar que la base de datos está online.

#### `pages/PlayerTracker.jsx` (Bloque 1)

El metajuego para el jugador. Usa `Promise.all` para lanzar 3 peticiones simultáneas a la API (`Profile`, `Progression`, `Sessions`). Renderiza las métricas clave (TTL, K/D) en tarjetas, dibuja el gráfico de `Recharts` para validar si el jugador mejora (H3), y despliega una tabla con el historial de partidas usando mapeo dinámico.

#### `pages/LabDashboard.jsx` (Bloque 2 y Matemáticas Espaciales)

El panel exclusivo para Game Designers. Valida la economía de objetos (H4) con un gráfico circular y pinta los Heatmaps de zonas de conflicto (H2).

* **El Reto Matemático:** Unity usa un plano `X/Z` donde el centro suele ser `(0,0)`, mientras que la web dibuja desde la esquina superior izquierda `(0,0)` hacia abajo en `X/Y`. Resolvemos esto traduciendo las coordenadas dinámicamente antes de dárselas a `simpleheat`:

```javascript
// Conversión matemática Unity -> Canvas Web
const scaleX = canvas.width / unityWidth;
const scaleZ = canvas.height / unityHeight;

let xWeb = (point.x - UNITY_MIN_X) * scaleX;
let yWeb = canvas.height - ((point.z - UNITY_MIN_Z) * scaleZ); 
// Restamos la Z al alto total para invertir el eje y alinear el Norte.

```

---

## 3. El Pipeline de Construcción (Resumen del Proceso)

La construcción siguió un método de integración progresiva de 5 pasos:

1. **Cimiento:** Inicialización de Vite y configuración de Tailwind CSS. Dockerización inmediata (`Dockerfile` multi-capa con Node.js) para asegurar que el entorno de desarrollo fuera idéntico al de producción, conectado a la misma red de Docker Compose que las APIs.
2. **Puente:** Creación de `api.js` aprovechando la variable de entorno `VITE_API_URL` expuesta por Docker.
3. **Esqueleto:** Configuración del enrutamiento de React (`<BrowserRouter>`) y creación de las vistas "placeholder" conectadas por el `Layout`.
4. **Tracker:** Ensamblaje del panel del jugador, gestionando los estados de carga (`loading`) y error (`error`) para ofrecer *feedback* visual (spinners) si la base de datos demora.
5. **Laboratorio:** Integración de gráficos complejos y resolución del conflicto de canvas con `heatmap.js`, culminando en la superposición perfecta de coordenadas de telemetría sobre el mapa de Unity estático (`/public/mapa_base.jpg`).

---

## 4. Ejecución del Proyecto

Al estar completamente dockerizado, levantar todo el ecosistema (Bases de Datos, Redis, Ingest API, Worker, Query API y Frontend React) requiere un único comando desde la raíz del proyecto:

```bash
docker-compose up --build

```

*El frontend estará disponible en `http://localhost:5173` y reaccionará a los cambios de código en tiempo real gracias a la sincronización de volúmenes de Docker.*
