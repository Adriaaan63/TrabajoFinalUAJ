import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Layout from './components/Layout';
import Home from './pages/Home';
import PlayerTracker from './pages/PlayerTracker';
import LabDashboard from './pages/LabDashboard';

function App() {
  return (
    <BrowserRouter>
      <Routes>
        {/* El Layout envuelve a todas las rutas */}
        <Route path="/" element={<Layout />}>
          <Route index element={<Home />} />
          <Route path="player/:id" element={<PlayerTracker />} />
          <Route path="lab" element={<LabDashboard />} />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

export default App;