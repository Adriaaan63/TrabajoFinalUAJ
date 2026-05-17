import { useState, useEffect, useRef } from 'react';
import { PieChart, Pie, Cell, Tooltip as RechartsTooltip, Legend, ResponsiveContainer } from 'recharts';
import { Target, Package, Crosshair, Map as MapIcon, Loader2 } from 'lucide-react';
import simpleheat from 'simpleheat';
import { api } from '../services/api';

// =====================================================================
// CONFIGURACIÓN DEL MAPA (¡Ajusta esto a tu escena de Unity!)
// =====================================================================
// Aquí defines los límites reales de tu nivel en coordenadas de Unity.
// Por ejemplo, si la esquina inferior izquierda de tu mapa en Unity está en X:-50, Z:-50
const UNITY_MIN_X = -55;
const UNITY_MAX_X = 55;
const UNITY_MIN_Z = -52;
const UNITY_MAX_Z = 52;

// Paleta de colores para el gráfico de tarta (Items)
const COLORS = ['#10b981', '#3b82f6', '#f59e0b', '#8b5cf6', '#ef4444'];

export default function LabDashboard() {
  const [loading, setLoading] = useState(true);
  const [summary, setSummary] = useState(null);
  const [itemsData, setItemsData] = useState([]);
  const [heatmapPoints, setHeatmapPoints] = useState([]);
  
  // Referencia al div donde heatmap.js inyectará su canvas
  const mapContainerRef = useRef(null);
  // Referencia para guardar la instancia del heatmap y no crear duplicados
  const heatmapInstanceRef = useRef(null);

  useEffect(() => {
    // Cargar todos los datos del laboratorio en paralelo
    Promise.all([
      api.getGlobalSummary(),
      api.getItemInteractions(),
      api.getDeathsHeatmap() // Limitado a 5000 por defecto en nuestra API
    ]).then(([summaryData, itemsResponse, deathsResponse]) => {
      
      setSummary(summaryData);
      
      // Formateamos los datos de items para que Recharts los entienda {name, value}
      const formattedItems = itemsResponse.by_item_type.map(item => ({
        name: item.item_type,
        value: item.pickup_count
      }));
      setItemsData(formattedItems);
      
      // Guardamos los puntos del heatmap (viene en el array .points del schema)
      setHeatmapPoints(deathsResponse.points || []);
      
      setLoading(false);
    }).catch(err => {
      console.error("Error cargando el laboratorio:", err);
      setLoading(false);
    });
  }, []);

  /// Efecto separado solo para dibujar el heatmap una vez que el canvas existe y hay datos
  useEffect(() => {
    if (!loading && mapContainerRef.current && heatmapPoints.length > 0) {
      const canvas = mapContainerRef.current;
      
      // CRÍTICO: Igualar la resolución interna del canvas a su tamaño en pantalla
      canvas.width = canvas.clientWidth;
      canvas.height = canvas.clientHeight;

      // 1. Inicializar simpleheat
      const heat = simpleheat(canvas);
      
      // Configuración de la "mancha" térmica: radius, blur
      heat.radius(25, 15);

      // 2. Lógica de Conversión Matemática (Unity -> Web)
      const unityWidth = UNITY_MAX_X - UNITY_MIN_X;
      const unityHeight = UNITY_MAX_Z - UNITY_MIN_Z;

      const scaleX = canvas.width / unityWidth;
      const scaleZ = canvas.height / unityHeight;

      // 3. Transformamos los puntos. simpleheat espera arrays: [x, y, value]
      const dataPoints = heatmapPoints.map(point => {
        let xWeb = (point.x - UNITY_MIN_X) * scaleX;
        let yWeb = canvas.height - ((point.z - UNITY_MIN_Z) * scaleZ);

        return [
          Math.floor(xWeb), 
          Math.floor(yWeb), 
          point.value || 1
        ];
      });

      // 4. Inyectar datos y dibujar
      heat.data(dataPoints);
      heat.max(5); // Intensidad máxima para que se vuelva rojo
      heat.draw();
    }
  }, [loading, heatmapPoints]);

  if (loading) {
    return (
      <div className="flex flex-col items-center justify-center min-h-[60vh] space-y-4">
        <Loader2 className="w-12 h-12 text-purple-500 animate-spin" />
        <p className="text-slate-400 font-mono">Compilando datos de investigación...</p>
      </div>
    );
  }

  return (
    <div className="space-y-8 animate-fadeIn">
      
      <div className="flex items-center gap-3 border-b border-slate-800 pb-4">
        <Crosshair className="w-8 h-8 text-purple-500" />
        <div>
          <h1 className="text-3xl font-black text-white tracking-wide uppercase">
            Laboratorio de Hipótesis
          </h1>
          <p className="text-slate-400">Validación empírica para Game Designers</p>
        </div>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
        
        {/* COLUMNA IZQUIERDA: Contexto y Validación H4 */}
        <div className="space-y-8 lg:col-span-1">
          
          {/* Métrica Global */}
          <div className="bg-slate-800/30 border border-slate-700/50 p-6 rounded-xl space-y-4">
            <h2 className="text-lg font-bold text-white flex items-center gap-2">
              <Target className="w-5 h-5 text-cyan-400" /> Contexto Global
            </h2>
            <div className="grid grid-cols-2 gap-4">
              <div className="bg-slate-900/50 p-4 rounded-lg">
                <div className="text-xs text-slate-400">Partidas Registradas</div>
                <div className="text-2xl font-mono text-white">{summary?.total_sessions || 0}</div>
              </div>
              <div className="bg-slate-900/50 p-4 rounded-lg">
                <div className="text-xs text-slate-400">K/D Medio Global</div>
                <div className="text-2xl font-mono text-rose-400">
                  {summary?.global_avg_kd?.toFixed(2) || '0.00'}
                </div>
              </div>
            </div>
          </div>

          {/* Gráfico Tarta: Hipótesis 4 (Economía) */}
          <div className="bg-slate-800/30 border border-slate-700/50 p-6 rounded-xl space-y-4 h-[400px] flex flex-col">
            <h2 className="text-lg font-bold text-white flex items-center gap-2">
              <Package className="w-5 h-5 text-purple-400" /> Hipótesis 4: Economía
            </h2>
            <p className="text-xs text-slate-400">
              ¿Ignoran los jugadores los recursos secundarios? (Distribución de recogidas de items).
            </p>
            <div className="flex-1 w-full min-h-0">
              {itemsData.length > 0 ? (
                <ResponsiveContainer width="100%" height="100%">
                  <PieChart>
                    <Pie
                      data={itemsData}
                      cx="50%"
                      cy="50%"
                      innerRadius={60}
                      outerRadius={80}
                      paddingAngle={5}
                      dataKey="value"
                    >
                      {itemsData.map((entry, index) => (
                        <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
                      ))}
                    </Pie>
                    <RechartsTooltip 
                      contentStyle={{ backgroundColor: '#0f172a', borderColor: '#334155', color: '#fff' }} 
                      itemStyle={{ color: '#fff' }}
                    />
                    <Legend />
                  </PieChart>
                </ResponsiveContainer>
              ) : (
                <div className="h-full flex items-center justify-center text-slate-500 text-sm italic">
                  Sin datos de items registrados.
                </div>
              )}
            </div>
          </div>

        </div>

        {/* COLUMNA DERECHA: El Heatmap (Validación H2) */}
        <div className="lg:col-span-2">
          <div className="bg-slate-800/30 border border-slate-700/50 p-6 rounded-xl h-full flex flex-col">
            <div className="mb-4">
              <h2 className="text-xl font-bold text-white flex items-center gap-2">
                <MapIcon className="w-6 h-6 text-rose-500" /> Hipótesis 2: Zonas de Conflicto (Heatmap)
              </h2>
              <p className="text-sm text-slate-400 mt-1">
                Visualización de mortalidad. Las zonas rojas indican cuellos de botella (Choke points) empíricos.
                Se muestran {heatmapPoints.length} eventos de muerte.
              </p>
            </div>

            {/* CONTENEDOR DEL MAPA */}
            {/* Aspect ratio 1:1 (cuadrado) o ajusta según las proporciones reales de tu mapa */}
            <div className="relative w-full aspect-square bg-slate-900 border-2 border-slate-800 rounded-lg overflow-hidden shadow-2xl">
              
              {/* Imagen de fondo del mapa de Unity */}
              <img 
                src="/mapa_base.png" 
                alt="Mapa del Nivel" 
                className="absolute inset-0 w-full h-full object-cover opacity-60 mix-blend-luminosity"
              />
              
              {/* Canvas nativo donde simpleheat pintará los datos */}
              <canvas 
                ref={mapContainerRef} 
                className="absolute inset-0 w-full h-full z-10"
              />
              
            </div>
          </div>
        </div>

      </div>
    </div>
  );
}