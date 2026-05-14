using UnityEngine;

public class ItemPickupHooks : MonoBehaviour
{
    [Header("Tipo de item")]
    public string itemType = "Health"; // "Health", "Weapon" o "Ammo"

    void OnDisable()
    {
        if (Tracker.Instance == null || !Tracker.Instance.IsReady) return;
        Tracker.Instance.TrackEvent(new Item_Picked(itemType));
    }
}