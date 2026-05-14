using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Configuracion del sistema de telemetria.
/// Se puede cargar desde un fichero JSON (para que sea editable en BUILD
/// sin recompilar) o usar los valores por defecto del Inspector como fallback.
///
/// Orden de prioridad al cargar:
///   1. Fichero JSON junto al ejecutable: {rutaBase}/tracker.config.json
///   2. Fichero JSON en persistentDataPath: {persistentDataPath}/tracker.config.json
///   3. Valores del Inspector del TrackerInitializer (si no se encuentra fichero).
/// </summary>
[Serializable]
public class TrackerConfig
{
    // ---------------- Flags maestros ----------------

    /// <summary>
    /// Si es false, el tracker no se inicializa y no se recogen eventos.
    /// Permite desactivar la telemetria por completo desde el config sin tocar codigo.
    /// </summary>
    public bool enabled = true;

    /// <summary>
    /// Si es true, vuelca al Debug.Log cada evento encolado y cada flush.
    /// Recomendado dejarlo en false en builds de produccion para no saturar el log.
    /// </summary>
    public bool verboseLogging = false;

    // ---------------- Serializacion ----------------

    /// <summary>Formato de serializacion: "JSON" o "CSV".</summary>
    public string serializer = "JSON";

    // ---------------- Persistencia ----------------

    /// <summary>Destino de las trazas: "LocalFile", "Firebase" o "Docker".</summary>
    public string persistence = "LocalFile";

    /// <summary>
    /// Modo de resolucion de la carpeta de salida para LocalFilePersistence:
    ///   - "NextToExe":      carpeta Telemetry/ junto al ejecutable (editor: junto a Assets/).
    ///   - "PersistentData": usa Application.persistentDataPath.
    ///   - "Custom":         usa la ruta absoluta definida en customOutputDir.
    /// </summary>
    public string localFileOutputMode = "NextToExe";

    /// <summary>
    /// Ruta absoluta personalizada, solo usada si localFileOutputMode == "Custom".
    /// Si la carpeta no existe, el tracker la crea.
    /// </summary>
    public string customOutputDir = "";

    /// <summary>
    /// Tamano maximo del archivo de traza en MB antes de rotar a uno nuevo.
    /// 0 o negativo = sin rotacion (un unico archivo por sesion).
    /// No aplica a Docker ni Firebase.
    /// </summary>
    public float fileRotationMaxMb = 0f;

    /// <summary>URL base de la Realtime Database de Firebase (solo si persistence == "Firebase").</summary>
    public string firebaseDatabaseUrl = "";

    // ---------------- Docker ----------------

    /// <summary>
    /// URL base de la API de ingestion en Docker (solo si persistence == "Docker").
    /// Ejemplo: "http://localhost:8000"
    /// El endpoint real sera: {dockerApiUrl}/upload_session
    /// </summary>
    public string dockerApiUrl = "http://localhost:8000";

    // ---------------- Flush / rendimiento ----------------

    /// <summary>Intervalo en segundos entre flushes automaticos de la cola a disco/red.</summary>
    public float autoFlushIntervalSeconds = 30f;

    /// <summary>
    /// Tamano maximo del buffer interno del FileStream en bytes (4096 por defecto).
    /// Solo aplica a LocalFile. Docker y Firebase gestionan su propio buffer.
    /// </summary>
    public int fileBufferSizeBytes = 4096;

    // ---------------- Filtrado de eventos ----------------

    /// <summary>
    /// Lista de nombres de clase de eventos a IGNORAR.
    /// Los eventos cuyo event_type este en esta lista se descartan antes de encolarse.
    /// Ejemplo: ["Player_Attack", "Heartbeat"] para desactivar esos dos.
    /// </summary>
    public List<string> disabledEventTypes = new List<string>();

    // ---------------- Carga / guardado ----------------

    /// <summary>Nombre del fichero de configuracion que se busca al arrancar.</summary>
    public const string CONFIG_FILE_NAME = "tracker.config.json";

    /// <summary>
    /// Carga la configuracion del sistema de ficheros siguiendo el orden de prioridad.
    /// Devuelve null si no encuentra nada (en cuyo caso loadedFromPath queda en null).
    /// </summary>
    public static TrackerConfig Load(out string loadedFromPath)
    {
        string nextToExe = Path.Combine(GetDirectoryNextToExe(), CONFIG_FILE_NAME);
        if (File.Exists(nextToExe))
        {
            TrackerConfig cfg = ReadFromFile(nextToExe);
            if (cfg != null) { loadedFromPath = nextToExe; return cfg; }
        }

        string persistent = Path.Combine(Application.persistentDataPath, CONFIG_FILE_NAME);
        if (File.Exists(persistent))
        {
            TrackerConfig cfg = ReadFromFile(persistent);
            if (cfg != null) { loadedFromPath = persistent; return cfg; }
        }

        loadedFromPath = null;
        return null;
    }

    /// <summary>
    /// Escribe la configuracion actual al fichero tracker.config.json junto al
    /// ejecutable. Util como herramienta de depuracion desde el Inspector.
    /// </summary>
    public void SaveToFileNextToExe(out string savedPath)
    {
        string dir = GetDirectoryNextToExe();
        Directory.CreateDirectory(dir);
        savedPath = Path.Combine(dir, CONFIG_FILE_NAME);
        File.WriteAllText(savedPath, JsonUtility.ToJson(this, true));
    }

    // ---------------- Helpers ----------------

    private static TrackerConfig ReadFromFile(string path)
    {
        try
        {
            string raw = File.ReadAllText(path);
            return JsonUtility.FromJson<TrackerConfig>(raw);
        }
        catch (Exception ex)
        {
            Debug.LogError($"[TrackerConfig] Error leyendo {path}: {ex.Message}");
            return null;
        }
    }

    public static string GetDirectoryNextToExe()
    {
        return Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
    }

    /// <summary>
    /// Resuelve a ruta absoluta la carpeta de salida segun el modo configurado.
    /// Crea la carpeta si no existe. Solo relevante para LocalFile.
    /// </summary>
    public string ResolveLocalOutputDir()
    {
        string dir;
        switch (localFileOutputMode)
        {
            case "PersistentData":
                dir = Path.Combine(Application.persistentDataPath, "Telemetry");
                break;
            case "Custom":
                if (string.IsNullOrWhiteSpace(customOutputDir))
                {
                    Debug.LogWarning("[TrackerConfig] customOutputDir esta vacio, "
                                     + "usando NextToExe como fallback.");
                    dir = Path.Combine(GetDirectoryNextToExe(), "Telemetry");
                }
                else
                {
                    dir = customOutputDir;
                }
                break;
            case "NextToExe":
            default:
                dir = Path.Combine(GetDirectoryNextToExe(), "Telemetry");
                break;
        }
        Directory.CreateDirectory(dir);
        return dir;
    }
}
