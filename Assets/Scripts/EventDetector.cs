using UnityEngine;
using Opsive.Shared.Events;

public class EventDetector : MonoBehaviour
{
    // Lista de eventos comunes de Opsive a probar
    private string[] eventosAProbar = new string[]
    {
        "OnHealthDamageReceived",
        "OnDamage",
        "OnTakeDamage",
        "OnHit",
        "OnHealthDamage",
        "OnCharacterDamage"
    };

    void Start()
    {
        // Registramos todos con un handler genérico
        foreach (var e in eventosAProbar)
        {
            try
            {
                EventHandler.RegisterEvent<float, Vector3, Vector3, GameObject, Collider>(
                    gameObject, e, (a, b, c, d, f) =>
                        Debug.Log($"[EVENTO DETECTADO] '{e}' | atacante: {d?.name} | tag: {d?.tag}"));
            }
            catch { }
        }
    }
}   