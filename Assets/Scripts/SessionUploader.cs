using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

public class SessionUploader : MonoBehaviour
{
    private string apiURL = "http://localhost:8000/upload_session";

    // Llamar a esto cuando termina la partida (desde el GameManager de Opsive)
    public void UploadAndEnd()
    {
        string json = TelemetryTracker.Instance.GetSessionJSON();
        StartCoroutine(PostSessionRoutine(json));
    }

    private IEnumerator PostSessionRoutine(string jsonContent)
    {
        using (UnityWebRequest request = new UnityWebRequest(apiURL, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonContent);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            Debug.Log("Enviando sesion...");
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
                Debug.Log(" Sesion enviada con exito");
            else
                Debug.LogError("Error al enviar: " + request.error);
        }
    }
}