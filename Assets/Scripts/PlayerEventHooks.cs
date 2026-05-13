using UnityEngine;
using Opsive.Shared.Events;

public class PlayerEventHooks : MonoBehaviour
{
    void Start()
    {
        EventHandler.RegisterEvent<Vector3, Vector3, GameObject>(gameObject, "OnDeath", OnPlayerDeath);
        EventHandler.RegisterEvent(gameObject, "OnRespawn", OnPlayerRespawn);
    }

    void OnDestroy()
    {
        EventHandler.UnregisterEvent<Vector3, Vector3, GameObject>(gameObject, "OnDeath", OnPlayerDeath);
        EventHandler.UnregisterEvent(gameObject, "OnRespawn", OnPlayerRespawn);
    }

    private void OnPlayerDeath(Vector3 position, Vector3 force, GameObject attacker)
    {
        if (TelemetryTracker.Instance == null) return;

        string killerId = "Unknown";
        try { killerId = attacker != null ? attacker.name : "Unknown"; }
        catch { killerId = "Unknown"; }

        TelemetryTracker.Instance.RegisterPlayerDeath(position.x, position.z, killerId);
        Debug.Log("[Telemetría] PLAYER murió. Killer: " + killerId);
    }

    private void OnPlayerRespawn()
    {
        if (TelemetryTracker.Instance == null) return;
        TelemetryTracker.Instance.RegisterSpawn(gameObject.name);
        Debug.Log("[Telemetría] PLAYER respawneado");
    }
}