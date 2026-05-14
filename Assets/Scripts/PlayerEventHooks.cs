using UnityEngine;
using Opsive.Shared.Events;
using System.Collections;
using Opsive.UltimateCharacterController.Items.Actions;

public class PlayerEventHooks : MonoBehaviour
{
    void Start()
    {
        EventHandler.RegisterEvent<Vector3, Vector3, GameObject>(gameObject, "OnDeath", OnPlayerDeath);
        EventHandler.RegisterEvent(gameObject, "OnRespawn", OnPlayerRespawn);
        EventHandler.RegisterEvent<IUsableItem>(gameObject, "OnItemUseComplete", OnShotFired);

        StartCoroutine(PositionHeartbeat());
    }

    void OnDestroy()
    {
        EventHandler.UnregisterEvent<Vector3, Vector3, GameObject>(gameObject, "OnDeath", OnPlayerDeath);
        EventHandler.UnregisterEvent(gameObject, "OnRespawn", OnPlayerRespawn);
        EventHandler.UnregisterEvent<IUsableItem>(gameObject, "OnItemUseComplete", OnShotFired);
    }

    private void OnPlayerDeath(Vector3 position, Vector3 force, GameObject attacker)
    {
        if (!Tracker.Instance.IsReady) return;
        string killerId = "Unknown";
        try { killerId = attacker != null ? attacker.name : "Unknown"; } catch { }
        Tracker.Instance.TrackEvent(new Player_Death(position.x, position.z, killerId));
    }

    private void OnPlayerRespawn()
    {
        if (!Tracker.Instance.IsReady) return;
        Tracker.Instance.TrackEvent(new Player_Spawn(gameObject.name));
    }

    private void OnShotFired(IUsableItem item)
    {
        if (!Tracker.Instance.IsReady) return;
        string weaponName = item != null ? item.GetType().Name : "Unknown";
        Tracker.Instance.TrackEvent(new Shot_Fired(weaponName));
    }

    private IEnumerator PositionHeartbeat()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            if (Tracker.Instance.IsReady)
            {
                Vector3 pos = transform.position;
                Tracker.Instance.TrackEvent(new Player_Position_Heartbeat(pos.x, pos.z));
            }
        }
    }
}