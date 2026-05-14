using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Bootstrap del sistema de telemetria. Se ejecuta al arranque y decide
/// como configurar el Tracker basandose en:
///
///   1. Un fichero tracker.config.json si existe (junto al ejecutable o
///      en persistentDataPath). Permite al playtester cambiar la
///      configuracion de una build SIN recompilar.
///
///   2. En su defecto, los valores que tengas puestos en el Inspector
///      (equivalente a la configuracion anterior, retrocompatible).
///
/// En el editor, el boton contextual "Generate config file next to exe"
/// vuelca los valores actuales del Inspector a un fichero JSON que luego
/// puedes distribuir junto con la build.
/// </summary>
public class TrackerInitializer : MonoBehaviour
{
    public enum S_type { JSON, CSV }

    // DOCKER agregado como nueva opcion de persistencia
    public enum P_type { LOCAL_FILE, FIREBASE, DOCKER }

    public enum OutputMode { NextToExe, PersistentData, Custom }

    [Header("--- Fallback (usado si NO existe tracker.config.json) ---")]
    [Tooltip("Si hay un fichero tracker.config.json junto al ejecutable o en "
             + "persistentDataPath, estos valores se IGNORAN y se usan los del fichero.")]
    public bool enabled = true;
    public bool verboseLogging = false;

    [Header("Serializacion y persistencia")]
    public S_type serializerType = S_type.JSON;
    public P_type persistanceType = P_type.LOCAL_FILE;

    [Header("Salida de ficheros locales")]
    [Tooltip("Solo aplica si persistenceType = LOCAL_FILE.\n"
             + "NextToExe: carpeta Telemetry/ junto al ejecutable (recomendado).\n"
             + "PersistentData: %APPDATA%/LocalLow/...\n"
             + "Custom: la ruta que escribas en 'customOutputDir'.")]
    public OutputMode localFileOutputMode = OutputMode.NextToExe;

    [Tooltip("Solo usado si localFileOutputMode = Custom")]
    public string customOutputDir = "";

    [Tooltip("Tamano maximo por archivo en MB antes de rotar (0 = sin rotacion).")]
    public float fileRotationMaxMb = 0f;

    [Header("Servidor Firebase (si persistencia = FIREBASE)")]
    public string firebaseDatabaseUrl =
        "https://featherrise-telemetry-p3-default-rtdb.europe-west1.firebasedatabase.app/";

    [Header("API Docker (si persistencia = DOCKER)")]
    [Tooltip("URL base del contenedor Docker con la API de ingestion.\n"
             + "Ejemplo: http://localhost:8000\n"
             + "El tracker llamara a {dockerApiUrl}/upload_session al cerrar la sesion.")]
    public string dockerApiUrl = "http://localhost:8000";

    [Header("Rendimiento")]
    [Tooltip("Intervalo en segundos entre flushes automaticos.\n"
             + "En modo Docker los flushes intermedios acumulan en memoria; "
             + "el envio real ocurre al cerrar la aplicacion.")]
    public float autoFlushIntervalSeconds = 30f;

    [Tooltip("Buffer interno del FileStream en bytes. Solo aplica a LOCAL_FILE.")]
    public int fileBufferSizeBytes = 4096;

    [Header("Eventos desactivados")]
    [Tooltip("Nombres de clase de eventos que NO se registraran (ej: Player_Attack).")]
    public List<string> disabledEventTypes = new List<string>();

    // ---------------------- ciclo de vida ----------------------

    void Start()
    {
        // 1. Cargar configuracion
        TrackerConfig config = TrackerConfig.Load(out string loadedPath);
        if (config == null)
        {
            config = BuildConfigFromInspector();
            Debug.Log("[TrackerInitializer] No se encontro tracker.config.json, "
                      + "usando valores del Inspector.");
        }
        else
        {
            Debug.Log($"[TrackerInitializer] Configuracion cargada desde: {loadedPath}");
        }

        // 2. Si el tracker esta desactivado, no construimos dependencias
        if (!config.enabled)
        {
            Tracker.Instance.Init(null, null, "", config);
            return;
        }

        // 3. Construir serializador y persistencia segun config
        string sessionId = System.Guid.NewGuid().ToString();
        ISerializer ser = ChooseSerializer(config, out string fileExtension);
        IPersistence pers = ChoosePersistence(config, sessionId, fileExtension);

        Tracker.Instance.Init(ser, pers, sessionId, config);
    }

    /// <summary>
    /// Copia los valores del Inspector a una instancia de TrackerConfig.
    /// Usado como fallback cuando no hay fichero de configuracion.
    /// </summary>
    private TrackerConfig BuildConfigFromInspector()
    {
        string persistenceStr = persistanceType switch
        {
            P_type.FIREBASE => "Firebase",
            P_type.DOCKER   => "Docker",
            _               => "LocalFile",
        };

        return new TrackerConfig
        {
            enabled                  = enabled,
            verboseLogging           = verboseLogging,
            serializer               = serializerType.ToString(),
            persistence              = persistenceStr,
            localFileOutputMode      = localFileOutputMode.ToString(),
            customOutputDir          = customOutputDir,
            fileRotationMaxMb        = fileRotationMaxMb,
            firebaseDatabaseUrl      = firebaseDatabaseUrl,
            dockerApiUrl             = dockerApiUrl,
            autoFlushIntervalSeconds = autoFlushIntervalSeconds,
            fileBufferSizeBytes      = fileBufferSizeBytes,
            disabledEventTypes       = new List<string>(disabledEventTypes ?? new List<string>()),
        };
    }

    private ISerializer ChooseSerializer(TrackerConfig cfg, out string extension)
    {
        // Firebase usa su propio serializador independientemente del campo serializer
        if (cfg.persistence == "Firebase")
        {
            extension = ".json";
            return new FirebaseSerializer();
        }

        // Docker siempre usa JSON (la API espera JSON)
        if (cfg.persistence == "Docker")
        {
            extension = ".json";
            return new JSONSerializer();
        }

        switch (cfg.serializer?.ToUpperInvariant())
        {
            case "CSV":
                extension = ".csv";
                return new CSVSerializer();
            case "JSON":
            default:
                extension = ".json";
                return new JSONSerializer();
        }
    }

    private IPersistence ChoosePersistence(TrackerConfig cfg, string sessionId,
                                           string extension)
    {
        switch (cfg.persistence)
        {
            case "Firebase":
                if (string.IsNullOrWhiteSpace(cfg.firebaseDatabaseUrl))
                {
                    Debug.LogWarning("[TrackerInitializer] Firebase URL vacia, cayendo a LocalFile.");
                    break;
                }
                return new FirebasePersistence(cfg.firebaseDatabaseUrl, sessionId);

            case "Docker":
                if (string.IsNullOrWhiteSpace(cfg.dockerApiUrl))
                {
                    Debug.LogWarning("[TrackerInitializer] dockerApiUrl vacia, cayendo a LocalFile.");
                    break;
                }
                Debug.Log($"[TrackerInitializer] Persistencia Docker -> {cfg.dockerApiUrl}/upload_session");
                return new DockerPersistence(cfg.dockerApiUrl, sessionId);
        }

        // LOCAL_FILE (default y fallback)
        string outDir = cfg.ResolveLocalOutputDir();
        Debug.Log($"[TrackerInitializer] Trazas locales en: {outDir}");
        return new LocalFilePersistence(outDir, sessionId, extension,
                                        cfg.fileRotationMaxMb,
                                        cfg.fileBufferSizeBytes);
    }

    // ---------------------- utilidad de editor ----------------------

    [ContextMenu("Generate config file next to exe (with current Inspector values)")]
    public void GenerateConfigFile()
    {
        TrackerConfig cfg = BuildConfigFromInspector();
        cfg.SaveToFileNextToExe(out string savedPath);
        Debug.Log($"[TrackerInitializer] Config escrito en: {savedPath}\n"
                  + "Puedes distribuir este fichero junto con la build. El usuario "
                  + "podra editarlo para cambiar la configuracion sin recompilar.");
    }
}
