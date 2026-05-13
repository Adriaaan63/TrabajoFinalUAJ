using UnityEngine;
using Opsive.Shared.Events;

public class PlayerEventHooks : MonoBehaviour
{
    // Esta variable nos dice si este script está en el JUGADOR o en una IA
    [Header("¿Es el jugador humano?")]
    public bool isHumanPlayer = true;

    void Start()
    {
        // Suscribirse con la firma correcta (3 parámetros)
        EventHandler.RegisterEvent<Vector3, Vector3, GameObject>(
            gameObject, "OnDeath", OnDeath);

        EventHandler.RegisterEvent(
            gameObject, "OnRespawn", OnRespawn);
    }

    void OnDestroy()
    {
        EventHandler.UnregisterEvent<Vector3, Vector3, GameObject>(
            gameObject, "OnDeath", OnDeath);

        EventHandler.UnregisterEvent(
            gameObject, "OnRespawn", OnRespawn);
    }

    // Ahora la firma tiene los 3 parámetros que Opsive manda
    private void OnDeath(Vector3 position, Vector3 force, GameObject attacker)
    {
        if (TelemetryTracker.Instance == null) return;

        string killerId = attacker != null ? attacker.name : "Unknown";

        if (isHumanPlayer)
        {
            // Murió el jugador
            TelemetryTracker.Instance.RegisterPlayerDeath(
                position.x, position.z, killerId);
        }
        else
        {
            // Murió una IA
            TelemetryTracker.Instance.RegisterAIDeath(
                position.x, position.z, killerId);
        }
    }

    private void OnRespawn()
    {
        if (TelemetryTracker.Instance == null) return;

        // Solo registramos el spawn del jugador humano
        if (isHumanPlayer)
        {
            TelemetryTracker.Instance.RegisterSpawn(gameObject.name);
        }
    }
}