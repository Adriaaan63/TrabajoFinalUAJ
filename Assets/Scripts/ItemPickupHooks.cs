using UnityEngine;
using Opsive.Shared.Events;

public class ItemPickupHooks : MonoBehaviour
{
    [Header("Tipo de item")]
    public string itemType = "Health"; // "Health", "Weapon" o "Ammo"

    void Start()
    {
        EventHandler.RegisterEvent<GameObject>(gameObject, "OnItemPickup", OnItemPickedUp);
    }

    void OnDestroy()
    {
        EventHandler.UnregisterEvent<GameObject>(gameObject, "OnItemPickup", OnItemPickedUp);
    }

    private void OnItemPickedUp(GameObject picker)
    {
        if (!Tracker.Instance.IsReady) return;
        if (picker != null && picker.CompareTag("Player"))
        {
            Tracker.Instance.TrackEvent(new Item_Picked(itemType));
        }
    }
}