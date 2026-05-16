/**
 * Capa de Servicios: Interacción con la Query API
 * * Usamos import.meta.env.VITE_API_URL para leer la variable de entorno que
 * configuramos en el docker-compose.yml. Si no existe, usamos el localhost por defecto.
 */
const API_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:8001/api/v1';

/**
 * Función auxiliar para hacer peticiones y manejar errores automáticamente.
 * Así no tenemos que repetir el "try/catch" y el "response.json()" en cada función.
 */
async function fetchWithError(url) {
    const response = await fetch(url);
    if (!response.ok) {
        // Si el jugador no existe (404) o hay error de BD (503), lanzamos un error claro
        throw new Error(`Error en la petición: ${response.status} ${response.statusText}`);
    }
    return response.json();
}

export const api = {
    // =====================================================================
    // BLOQUE 1: TRACKER DEL JUGADOR
    // =====================================================================

    // Obtiene el perfil acumulado (K/D, accuracy, etc.)
    getPlayerProfile: (playerId) => 
        fetchWithError(`${API_BASE_URL}/players/${playerId}`),

    // Obtiene el historial de partidas (soporta paginación)
    getPlayerSessions: (playerId, limit = 20, offset = 0) => 
        fetchWithError(`${API_BASE_URL}/players/${playerId}/sessions?limit=${limit}&offset=${offset}`),

    // Obtiene la curva temporal de K/D y precisión (Valida H3)
    getPlayerProgression: (playerId, limit = 50) => 
        fetchWithError(`${API_BASE_URL}/players/${playerId}/progression?limit=${limit}`),


    // =====================================================================
    // BLOQUE 2: ANÁLISIS / INVESTIGACIÓN (Dashboard Interno)
    // =====================================================================

    // Resumen global para la cabecera del dashboard
    getGlobalSummary: () => 
        fetchWithError(`${API_BASE_URL}/metrics/global-summary`),

    // Recogidas de objetos (Valida H4)
    getItemInteractions: (playerId = null) => {
        const url = playerId 
            ? `${API_BASE_URL}/metrics/item-interactions?player_id=${playerId}`
            : `${API_BASE_URL}/metrics/item-interactions`;
        return fetchWithError(url);
    },

    // Histograma de Time-to-Live (Valida H1)
    getTtlDistribution: (bucketSizeSeconds = 5, playerId = null) => {
        const params = new URLSearchParams({ bucket_size_seconds: bucketSizeSeconds });
        if (playerId) params.append('player_id', playerId);
        return fetchWithError(`${API_BASE_URL}/metrics/ttl-distribution?${params.toString()}`);
    },

    // Coordenadas de muertes (Valida H2)
    getDeathsHeatmap: (floorId = null, limit = 5000, includeAi = true) => {
        const params = new URLSearchParams({ limit, include_ai_deaths: includeAi });
        if (floorId !== null) params.append('floor_id', floorId);
        return fetchWithError(`${API_BASE_URL}/heatmaps/deaths?${params.toString()}`);
    },

    // Coordenadas de tránsito (Valida H2)
    getNavigationHeatmap: (floorId = null, limit = 10000) => {
        const params = new URLSearchParams({ limit });
        if (floorId !== null) params.append('floor_id', floorId);
        return fetchWithError(`${API_BASE_URL}/heatmaps/navigation?${params.toString()}`);
    }
};