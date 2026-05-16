import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { Search, Server, Activity } from 'lucide-react';
import { api } from '../services/api';

export default function Home() {
  const [playerId, setPlayerId] = useState('');
  const [globalStats, setGlobalStats] = useState(null);
  const navigate = useNavigate();

  // Comprobamos si el Paso 2 funciona llamando al backend
  useEffect(() => {
    api.getGlobalSummary()
      .then(data => setGlobalStats(data))
      .catch(err => console.error("Error al conectar con la API:", err));
  }, []);

  const handleSearch = (e) => {
    e.preventDefault();
    if (playerId.trim()) {
      navigate(`/player/${playerId.trim()}`);
    }
  };

  return (
    <div className="flex flex-col items-center justify-center min-h-[80vh] space-y-12">
      {/* Título Principal */}
      <div className="text-center space-y-4">
        <h1 className="text-5xl font-black tracking-tight text-transparent bg-clip-text bg-gradient-to-r from-cyan-400 to-blue-600">
          OPSIVE TELEMETRY HUB
        </h1>
        <p className="text-slate-400 text-lg">Introduce tu ID de jugador para acceder a tu Tracker</p>
      </div>

      {/* Barra de Búsqueda */}
      <form onSubmit={handleSearch} className="relative w-full max-w-md">
        <div className="relative flex items-center">
          <Search className="absolute left-4 text-cyan-500 w-5 h-5" />
          <input
            type="text"
            placeholder="Ej: Player_12345"
            value={playerId}
            onChange={(e) => setPlayerId(e.target.value)}
            className="w-full bg-slate-800/50 border border-slate-700 text-white rounded-lg py-4 pl-12 pr-4 focus:outline-none focus:ring-2 focus:ring-cyan-500 transition-all placeholder:text-slate-500 font-mono"
          />
          <button 
            type="submit"
            className="absolute right-2 bg-cyan-600 hover:bg-cyan-500 text-white px-4 py-2 rounded-md font-semibold transition-colors"
          >
            Buscar
          </button>
        </div>
      </form>

      {/* Widget de Estado del Servidor (Prueba de la API) */}
      <div className="flex items-center gap-8 pt-8 border-t border-slate-800/50">
        <div className="flex items-center gap-3 text-slate-300">
          <Server className="w-5 h-5 text-emerald-400" />
          <span>Servidor Activo</span>
        </div>
        <div className="flex items-center gap-3 text-slate-300">
          <Activity className="w-5 h-5 text-blue-400" />
          <span>
            {globalStats 
              ? `${globalStats.total_sessions} Partidas Registradas` 
              : 'Conectando con DB...'}
          </span>
        </div>
      </div>
    </div>
  );
}