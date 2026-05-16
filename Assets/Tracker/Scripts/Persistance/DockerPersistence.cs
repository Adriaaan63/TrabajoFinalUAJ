using System;
using System.Net.Http;
using System.Text;
using UnityEngine;

/// <summary>
/// Persistencia via HTTP hacia la API de ingestion en Docker.
/// 
/// Estrategia de envio: acumula TODOS los datos en memoria durante la sesion
/// y los envia como un unico POST al finalizar (OnApplicationQuit -> Close()).
/// Esto es coherente con el endpoint /upload_session, que espera la sesion
/// completa de una vez (batch), no evento a evento.
///
/// Nota: Save() es thread-safe porque StringBuilder no se usa desde varios
/// hilos simultaneamente en este flujo (el Tracker serializa antes de llamar).
/// </summary>
public class DockerPersistence : IPersistence
{
    private readonly string _uploadUrl;
    private readonly string _sessionId;
    private readonly StringBuilder _buffer = new StringBuilder();

    // Reutilizamos HttpClient estatico igual que FirebasePersistence
    private static readonly HttpClient _httpClient = new HttpClient();

    /// <param name="baseUrl">URL base del contenedor Docker, ej: "http://localhost:8000"</param>
    /// <param name="sessionId">ID unico de la sesion (GUID generado en TrackerInitializer)</param>
    public DockerPersistence(string baseUrl, string sessionId)
    {
        if (string.IsNullOrWhiteSpace(baseUrl))
            throw new ArgumentException("[DockerPersistence] dockerApiUrl no puede estar vacio.");

        _uploadUrl = baseUrl.TrimEnd('/') + "/upload_session";
        _sessionId = sessionId;
    }

    /// <summary>
    /// Guarda la cabecera del serializador (tipicamente "[" en JSON).
    /// Limpia el buffer por si se llama Open() mas de una vez.
    /// </summary>
    public void Open(string header)
    {
        _buffer.Clear();
        if (!string.IsNullOrEmpty(header))
            _buffer.Append(header);

        Debug.Log($"[DockerPersistence] Buffer listo. Enviara a: {_uploadUrl}");
    }

    /// <summary>
    /// Acumula el batch serializado en el buffer. NO hace ninguna llamada de red.
    /// El envio ocurre unicamente en Close() para garantizar la sesion completa.
    /// </summary>
    public void Save(string data)
    {
        if (string.IsNullOrEmpty(data)) return;
        _buffer.Append(data);
    }

    /// <summary>
    /// Cierra el buffer con el footer del serializador (tipicamente "]") y envia
    /// la sesion completa al endpoint de Docker como un POST JSON sincrono.
    /// Se llama desde OnApplicationQuit con forceSynchronous=true, por lo que
    /// bloquear el hilo aqui es el comportamiento esperado y correcto.
    /// </summary>
    public void Close(string footer)
    {
        if (!string.IsNullOrEmpty(footer))
            _buffer.Append(footer);

        string eventsJson = _buffer.ToString();

        if (string.IsNullOrWhiteSpace(eventsJson))
        {
            Debug.LogWarning("[DockerPersistence] Buffer vacio, no se envia nada.");
            return;
        }

        // Envolvemos el array de eventos en el envelope que espera la API
        // Coincide con la estructura de session_data.json del documento 01_API_Unity_To_Docker.md
        string payload = BuildPayload(eventsJson);

        try
        {
            Debug.Log($"[DockerPersistence] Enviando sesion a {_uploadUrl} ...");
            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            // .Result bloquea el hilo: correcto en OnApplicationQuit donde
            // el Tracker ya forzó forceSynchronous=true (mismo patron que FirebasePersistence)
            HttpResponseMessage response = _httpClient.PostAsync(_uploadUrl, content).Result;

            if (response.IsSuccessStatusCode)
            {
                Debug.Log("[DockerPersistence] Sesion enviada correctamente.");
            }
            else
            {
                throw new Exception(
                    $"HTTP {(int)response.StatusCode} {response.ReasonPhrase}");
            }
        }
        catch (Exception ex)
        {
            // Logueamos pero NO re-lanzamos: en OnApplicationQuit el proceso
            // va a cerrar de todos modos y no tiene sentido reencolar.
            Debug.LogError($"[DockerPersistence] Error al enviar sesion: {ex.Message}");
        }
        finally
        {
            _buffer.Clear();
        }
    }

    // ----------------------------------------------------------------

    private string BuildPayload(string eventsJsonArray)
    {
        // Formato esperado por /upload_session (ver 01_API_Unity_To_Docker.md):
        // {
        //   "session_id": "...",
        //   "player_id":  "...",   <- usamos session_id como player_id por defecto
        //   "events":     [...]
        // }
        return "{" +
               $"\"session_id\":\"{EscapeJson(_sessionId)}\"," +
               $"\"events\":{eventsJsonArray}" +
               "}";
    }

    /// <summary>Escapa caracteres especiales de JSON en strings simples.</summary>
    private static string EscapeJson(string s)
    {
        return s?.Replace("\\", "\\\\").Replace("\"", "\\\"") ?? "";
    }
}
