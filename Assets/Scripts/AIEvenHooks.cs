using UnityEngine;
using Opsive.Shared.Events;

public class AIEventHooks : MonoBehaviour
{
    void Start()
    {
        EventHandler.RegisterEvent<Vector3, Vector3, GameObject>(gameObject, "OnDeath", OnAIDeath);
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
        if (!Tracker.Instance.IsReady) return;
        string killerId = "Unknown";
        try { killerId = attacker != null ? attacker.name : "Unknown"; } catch { }
        Tracker.Instance.TrackEvent(new AI_Death(position.x, position.z, killerId));
    }

    private void OnDamageReceived(float amount, Vector3 pos, Vector3 force,
                                   GameObject attacker, Collider hitCollider)
    {
        if (!Tracker.Instance.IsReady) return;
        if (attacker == null) return;
        try
        {
            if (attacker.CompareTag("Player"))
            {
                string zone = hitCollider != null &&
                              hitCollider.name.ToLower().Contains("head") ? "Head" : "Body";
                Tracker.Instance.TrackEvent(new Shot_Hit(zone));
            }
        }
        catch { }
    }
}