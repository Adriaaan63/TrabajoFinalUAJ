import { useState, useEffect } from 'react';
import { useParams, Link } from 'react-router-dom';
import { 
  Skull, Target, Clock, Package, Shield, 
  ArrowLeft, Calendar, Swords, Trophy, BarChart2 
} from 'lucide-react';
import { 
  LineChart, Line, XAxis, YAxis, CartesianGrid, 
  Tooltip, Legend, ResponsiveContainer 
} from 'recharts';
import { api } from '../services/api';

export default function PlayerTracker() {
  const { id } = useParams(); // Captura el player_id de la URL
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  
  // Estados para almacenar los 3 bloques de datos requeridos
  const [profile, setProfile] = useState(null);
  const [progression, setProgression] = useState([]);
  const [sessions, setSessions] = useState([]);

  useEffect(() => {
    setLoading(true);
    setError(null);

    // Ejecutamos las 3 llamadas en paralelo para un rendimiento óptimo
    Promise.all([
      api.getPlayerProfile(id),
      api.getPlayerProgression(id),
      api.getPlayerSessions(id, 10, 0) // Traemos las últimas 10 sesiones
    ])
    .then(([profileData, progressionData, sessionsData]) => {
      setProfile(profileData);
      setProgression(progressionData);
      setSessions(sessionsData);
      setLoading(false);
    })
    .catch((err) => {
      console.error(err);
      setError("No se pudieron cargar las estadísticas de este jugador. Verifica que el ID existe.");
      setLoading(false);
    });
  }, [id]);

  if (loading) {
    return (
      <div className="flex flex-col items-center justify-center min-h-[60vh] space-y-4">
        <div className="w-12 h-12 border-4 border-cyan-500 border-t-transparent rounded-full animate-spin"></div>
        <p className="text-slate-400 font-mono">Buscando registros en PostgreSQL...</p>
      </div>
    );
  }

  if (error) {
    return (
      <div className="text-center py-12 space-y-6">
        <div className="bg-red-500/10 border border-red-500/30 text-red-400 p-4 rounded-lg max-w-md mx-auto">
          {error}
        </div>
        <Link to="/" className="inline-flex items-center gap-2 text-cyan-400 hover:underline">
          <ArrowLeft className="w-4 h-4" /> Volver al buscador
        </Link>
      </div>
    );
  }

  return (
    <div className="space-y-8 animate-fadeIn">
      {/* Cabecera del Perfil */}
      <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4 pb-4 border-b border-slate-800">
        <div className="space-y-1">
          <Link to="/" className="flex items-center gap-2 text-xs text-slate-400 hover:text-cyan-400 transition-colors mb-2">
            <ArrowLeft className="w-3 h-3" /> CAMBIAR DE JUGADOR
          </Link>
          <h1 className="text-3xl font-black tracking-wide text-white uppercase font-mono flex items-center gap-3">
            <Shield className="text-cyan-400 w-8 h-8" /> {profile.player_id}
          </h1>
          <p className="text-slate-400 text-sm">
            Historial consolidado a través de {profile.total_sessions} sesiones de combate
          </p>
        </div>
        <div className="bg-slate-800/40 border border-slate-700/50 px-4 py-3 rounded-lg flex items-center gap-4">
          <Trophy className="text-yellow-500 w-5 h-5" />
          <div>
            <div className="text-xs text-slate-400">Tiempo Total</div>
            <div className="text-lg font-bold font-mono">
              {Math.round(profile.total_playtime_seconds / 60)} min
            </div>
          </div>
        </div>
      </div>

      {/* BLOQUE A: Tarjetas de Resumen (Métricas Clave) */}
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4">
        {/* Tarjeta K/D Ratio */}
        <div className="bg-slate-800/30 border border-slate-800 p-5 rounded-xl space-y-3 relative overflow-hidden group hover:border-cyan-500/30 transition-colors">
          <div className="flex justify-between items-start">
            <span className="text-xs font-bold text-slate-400 tracking-wider">M3.1 · RATIO K/D</span>
            <Skull className="text-rose-500 w-5 h-5" />
          </div>
          <div className="text-3xl font-black font-mono text-white">
            {profile.kd_ratio.toFixed(2)}
          </div>
          <div className="text-xs text-slate-500 flex justify-between">
            <span>Bajas: {profile.total_kills}</span>
            <span>Muertes: {profile.total_deaths}</span>
          </div>
          <div className="absolute bottom-0 left-0 h-1 bg-rose-500 w-full opacity-50"></div>
        </div>

        {/* Tarjeta Tasa de Precisión */}
        <div className="bg-slate-800/30 border border-slate-800 p-5 rounded-xl space-y-3 relative overflow-hidden group hover:border-cyan-500/30 transition-colors">
          <div className="flex justify-between items-start">
            <span className="text-xs font-bold text-slate-400 tracking-wider">M3.2 · PRECISIÓN MEDIA</span>
            <Target className="text-cyan-400 w-5 h-5" />
          </div>
          <div className="text-3xl font-black font-mono text-white">
            {(profile.avg_accuracy * 100).toFixed(1)}%
          </div>
          <div className="text-xs text-slate-500">
            Porcentaje de acierto en impactos físicos de bala
          </div>
          <div className="absolute bottom-0 left-0 h-1 bg-cyan-500 w-full opacity-50"></div>
        </div>

        {/* Tarjeta Time-to-Live (TTL) */}
        <div className="bg-slate-800/30 border border-slate-800 p-5 rounded-xl space-y-3 relative overflow-hidden group hover:border-cyan-500/30 transition-colors">
          <div className="flex justify-between items-start">
            <span className="text-xs font-bold text-slate-400 tracking-wider">M1.1 · TIME-TO-LIVE MEDIO</span>
            <Clock className="text-amber-400 w-5 h-5" />
          </div>
          <div className="text-3xl font-black font-mono text-white">
            {profile.avg_ttl_seconds.toFixed(1)}s
          </div>
          <div className="text-xs text-slate-500">
            Supervivencia estimada por cada ciclo de Spawn
          </div>
          <div className="absolute bottom-0 left-0 h-1 bg-amber-500 w-full opacity-50"></div>
        </div>

        {/* Tarjeta Interacción Entorno */}
        <div className="bg-slate-800/30 border border-slate-800 p-5 rounded-xl space-y-3 relative overflow-hidden group hover:border-cyan-500/30 transition-colors">
          <div className="flex justify-between items-start">
            <span className="text-xs font-bold text-slate-400 tracking-wider">M4.1 · RECOGIDA DE ITEMS</span>
            <Package className="text-purple-400 w-5 h-5" />
          </div>
          <div className="text-3xl font-black font-mono text-white">
            {profile.items_picked}
          </div>
          <div className="text-xs text-slate-500">
            Interacciones con armas, salud y munición del mapa
          </div>
          <div className="absolute bottom-0 left-0 h-1 bg-purple-500 w-full opacity-50"></div>
        </div>
      </div>

      {/* BLOQUE B: Gráfica de Progresión (Validación de la Hipótesis 3) */}
      <div className="bg-slate-800/20 border border-slate-800 p-6 rounded-xl space-y-4">
        <div className="flex items-center gap-2">
          <BarChart2 className="w-5 h-5 text-cyan-400" />
          <h2 className="text-lg font-bold text-white tracking-wide">
            Curva de Aprendizaje y Evolución Temporal (Hipótesis 3)
          </h2>
        </div>
        <p className="text-xs text-slate-400 max-w-3xl">
          Esta gráfica traza el progreso cronológico del jugador. Una tendencia ascendente en las líneas 
          corrobora empíricamente que la visualización de estadísticas incentiva la mejora de rendimiento.
        </p>

        <div className="h-80 w-full pt-4">
          {progression.length > 0 ? (
            <ResponsiveContainer width="100%" h="100%">
              <LineChart data={progression} margin={{ top: 5, right: 30, left: 20, bottom: 5 }}>
                <CartesianGrid strokeDasharray="3 3" stroke="#1e293b" />
                <XAxis 
                  dataKey="session_number" 
                  stroke="#64748b" 
                  tickFormatter={(val) => `Partida ${val}`}
                  style={{ fontSize: '12px', fontFamily: 'monospace' }}
                />
                <YAxis yAxisId="left" stroke="#f43f5e" style={{ fontSize: '12px' }} />
                <YAxis yAxisId="right" orientation="right" stroke="#06b6d4" style={{ fontSize: '12px' }} tickFormatter={(val) => `${(val * 100).toFixed(0)}%`} />
                <Tooltip 
                  contentStyle={{ backgroundColor: '#0f172a', borderColor: '#334155', color: '#fff' }}
                  labelFormatter={(label) => `Sesión Histórica N° ${label}`}
                />
                <Legend />
                <Line yAxisId="left" type="monotone" dataKey="kd_ratio" name="Ratio K/D (M3.1)" stroke="#f43f5e" strokeWidth={3} activeDot={{ r: 8 }} />
                <Line yAxisId="right" type="monotone" dataKey="accuracy" name="Precisión (M3.2)" stroke="#06b6d4" strokeWidth={2} />
              </LineChart>
            </ResponsiveContainer>
          ) : (
            <div className="h-full flex items-center justify-center text-slate-500 text-sm italic">
              Datos de progresión insuficientes para renderizar la curva.
            </div>
          )}
        </div>
      </div>

      {/* BLOQUE C: Historial de Sesiones (Tabla Detallada) */}
      <div className="bg-slate-800/20 border border-slate-800 rounded-xl overflow-hidden">
        <div className="p-6 border-b border-slate-800 flex items-center gap-2">
          <Swords className="w-5 h-5 text-rose-500" />
          <h2 className="text-lg font-bold text-white tracking-wide">Registro de Partidas Recientes</h2>
        </div>
        
        <div className="overflow-x-auto">
          <table className="w-full text-left border-collapse">
            <thead>
              <tr className="border-b border-slate-800 bg-slate-800/40 text-slate-400 text-xs font-semibold uppercase tracking-wider font-mono">
                <th className="py-4 px-6">Fecha / Inicio</th>
                <th className="py-4 px-6">Duración</th>
                <th className="py-4 px-6 text-rose-400">Bajas</th>
                <th className="py-4 px-6 text-slate-400">Muertes</th>
                <th className="py-4 px-6">K/D</th>
                <th className="py-4 px-6 text-cyan-400">Precisión</th>
                <th className="py-4 px-6 text-amber-400">TTL Medio</th>
                <th className="py-4 px-6 text-purple-400">Items</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-slate-800/60 font-mono text-sm text-slate-300">
              {sessions.map((session) => (
                <tr key={session.session_id} className="hover:bg-slate-800/30 transition-colors">
                  <td className="py-4 px-6 flex items-center gap-2 text-slate-400">
                    <Calendar className="w-4 h-4 text-slate-500" />
                    {new Date(session.started_at).toLocaleDateString()} {new Date(session.started_at).toLocaleTimeString([], {hour: '2-digit', minute:'2-digit'})}
                  </td>
                  <td className="py-4 px-6">{session.duration_seconds}s</td>
                  <td className="py-4 px-6 font-bold text-white">{session.kills}</td>
                  <td className="py-4 px-6">{session.deaths}</td>
                  <td className="py-4 px-6 font-bold">{session.kd_ratio.toFixed(2)}</td>
                  <td className="py-4 px-6">{(session.accuracy * 100).toFixed(0)}%</td>
                  <td className="py-4 px-6">{session.avg_ttl_seconds.toFixed(1)}s</td>
                  <td className="py-4 px-6">{session.items_picked}</td>
                </tr>
              ))}
              {sessions.length === 0 && (
                <tr>
                  <td colSpan="8" className="py-8 text-center text-slate-500 italic">
                    Este jugador no registra sesiones completadas en la base de datos.
                  </td>
                </tr>
              )}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
}