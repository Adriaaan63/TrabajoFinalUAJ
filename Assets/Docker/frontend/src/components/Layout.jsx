import { Outlet, Link, useLocation } from 'react-router-dom';
import { Crosshair, FlaskConical, Home } from 'lucide-react';

export default function Layout() {
  const location = useLocation();

  // Función auxiliar para saber qué pestaña está activa y resaltarla
  const isActive = (path) => location.pathname === path;

  return (
    <div className="min-h-screen bg-slate-900 text-slate-100 font-sans selection:bg-cyan-500/30">
      {/* Barra de Navegación Superior */}
      <nav className="sticky top-0 z-50 border-b border-slate-800 bg-slate-900/80 backdrop-blur-md">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="flex items-center justify-between h-16">
            
            {/* Logo a la izquierda */}
            <div className="flex items-center gap-2">
              <Crosshair className="text-cyan-400 w-6 h-6" />
              <span className="font-bold text-xl tracking-wider text-white">FPS<span className="text-cyan-400">.</span>STATS</span>
            </div>

            {/* Enlaces a la derecha */}
            <div className="flex space-x-4">
              <Link 
                to="/" 
                className={`flex items-center gap-2 px-3 py-2 rounded-md text-sm font-medium transition-colors ${isActive('/') ? 'bg-slate-800 text-cyan-400' : 'text-slate-300 hover:bg-slate-800 hover:text-white'}`}
              >
                <Home className="w-4 h-4" /> Inicio
              </Link>
              
              <Link 
                to="/lab" 
                className={`flex items-center gap-2 px-3 py-2 rounded-md text-sm font-medium transition-colors ${isActive('/lab') ? 'bg-slate-800 text-purple-400' : 'text-slate-300 hover:bg-slate-800 hover:text-white'}`}
              >
                <FlaskConical className="w-4 h-4" /> Lab (Devs)
              </Link>
            </div>
            
          </div>
        </div>
      </nav>

      {/* Contenedor principal donde se cargan las páginas */}
      <main className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        <Outlet />
      </main>
    </div>
  );
}