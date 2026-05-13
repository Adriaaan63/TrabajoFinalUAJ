using UnityEngine;
using Opsive.Shared.Events;
using Opsive.UltimateCharacterController.Items.Actions;

public class WeaponEventHooks : MonoBehaviour
{
    void Start()
    {
        // Opsive lanza este evento cada vez que se dispara
        EventHandler.RegisterEvent<object>(gameObject, "OnItemUseComplete", OnShotFired);

        // Impacto detectado por el sistema de daño
        EventHandler.RegisterEvent<float, Vector3, Vector3, GameObject, Collider>(
            gameObject, "OnHealthDamageReceived", OnDamageDealt);
    }

    void OnDestroy()
    {
        EventHandler.UnregisterEvent<object>(gameObject, "OnItemUseComplete", OnShotFired);
        EventHandler.UnregisterEvent<float, Vector3, Vector3, GameObject, Collider>(
            gameObject, "OnHealthDamageReceived", OnDamageDealt);
    }

    private void OnShotFired(object itemAction)
    {
        if (TelemetryTracker.Instance == null) return;
        // Intentamos obtener el nombre del arma
        string weaponName = itemAction != null ? itemAction.GetType().Name : "Unknown";
        TelemetryTracker.Instance.RegisterShotFired(weaponName);
    }

    // Este evento va en las IAs/jugador que RECIBE el daño, no en quien dispara
    // Por eso conviene ponerlo en los enemigos también
    private void OnDamageDealt(float amount, Vector3 pos, Vector3 force,
                                GameObject attacker, Collider hitCollider)
    {
        if (TelemetryTracker.Instance == null) return;

        // Solo registrar si el atacante es el jugador
        if (attacker != null && attacker.CompareTag("Player"))
        {
            // Detectar si es cabeza según el nombre del collider
            string zone = hitCollider != null &&
                          hitCollider.name.ToLower().Contains("head") ? "Head" : "Body";
            TelemetryTracker.Instance.RegisterShotHit(zone);
        }
    }
}