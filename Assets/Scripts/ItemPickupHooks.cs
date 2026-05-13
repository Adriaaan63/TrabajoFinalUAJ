using UnityEngine;

public class ItemPickupHooks : MonoBehaviour
{
    [Header("Tipo de item")]
    public string itemType = "Health"; // "Health", "Weapon" o "Ammo"

    // Opsive desactiva el objeto cuando se recoge
    // OnDisable se llama automáticamente en ese momento
    void OnDisable()
    {
        if (TelemetryTracker.Instance == null) return;
        TelemetryTracker.Instance.RegisterItemPicked(itemType);
    }
}