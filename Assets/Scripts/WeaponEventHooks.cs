using UnityEngine;
using Opsive.Shared.Events;
using Opsive.UltimateCharacterController.Items.Actions;

public class WeaponEventHooks : MonoBehaviour
{
    void Start()
    {
        EventHandler.RegisterEvent<IUsableItem>(gameObject, "OnItemUseComplete", OnShotFired);
        Debug.Log("[Telemetría] WeaponEventHooks registrado en: " + gameObject.name);
    }

    void OnDestroy()
    {
        EventHandler.UnregisterEvent<IUsableItem>(gameObject, "OnItemUseComplete", OnShotFired);
    }

    private void OnShotFired(IUsableItem item)
    {
        if (TelemetryTracker.Instance == null) return;
        string weaponName = item != null ? item.GetType().Name : "Unknown";
        TelemetryTracker.Instance.RegisterShotFired(weaponName);
        Debug.Log("[Telemetría] Shot_Fired: " + weaponName);
    }
}