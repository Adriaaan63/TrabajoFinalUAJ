using UnityEngine;
using Opsive.Shared.Events;

public class AIEventHooks : MonoBehaviour
{
    void Start()
    {
        EventHandler.RegisterEvent<Vector3, Vector3, GameObject>(gameObject, "OnDeath", OnAIDeath);

        // Escuchar cuando esta IA recibe daño
        EventHandler.RegisterEvent<float, Vector3, Vector3, GameObject, Collider>(
            gameObject, "OnHealthDamageReceived", OnDamageReceived);
    }

    void OnDestroy()
    {
        EventHandler.UnregisterEvent<Vector3, Vector3, GameObject>(gameObject, "OnDeath", OnAIDeath);
        EventHandler.UnregisterEvent<float, Vector3, Vector3, GameObject, Collider>(
            gameObject, "OnHealthDamageReceived", OnDamageReceived);
    }

    private void OnAIDeath(Vector3 position, Vector3 force, GameObject attacker)
    {
        if (TelemetryTracker.Instance == null) return;

        // Protección contra null
        string killerId = "Unknown";
        try { killerId = attacker != null ? attacker.name : "Unknown"; }
        catch { killerId = "Unknown"; }

        TelemetryTracker.Instance.RegisterAIDeath(position.x, position.z, killerId);
        Debug.Log("[Telemetría] IA muerta. Killer: " + killerId);
    }

    private void OnDamageReceived(float amount, Vector3 pos, Vector3 force,
                                   GameObject attacker, Collider hitCollider)
    {
        if (TelemetryTracker.Instance == null) return;
        if (attacker == null) return; // <-- esto evita el crash

        try
        {
            if (attacker.CompareTag("Player"))
            {
                string zone = hitCollider != null &&
                              hitCollider.name.ToLower().Contains("head") ? "Head" : "Body";
                TelemetryTracker.Instance.RegisterShotHit(zone);
                Debug.Log("[Telemetría] Shot_Hit en zona: " + zone);
            }
        }
        catch { /* ignorar si Opsive lanza algo interno */ }
    }
}