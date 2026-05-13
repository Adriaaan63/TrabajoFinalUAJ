using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelemetryTracker : MonoBehaviour
{
    public static TelemetryTracker Instance;

    private List<TelemetryEvent> events = new List<TelemetryEvent>();
    private string sessionId;
    private float sessionStartTime;

    public string playerId = "Jugador_01";

    void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    void Start()
    {
        sessionId = Guid.NewGuid().ToString();
        sessionStartTime = Time.time;
        StartCoroutine(PositionHeartbeat());
        Debug.Log("[Telemetría] Sesión iniciada: " + sessionId);
    }

    // ---- Métodos públicos ----

    public void RegisterSpawn(string spawnPointId)
    {
        events.Add(new TelemetryEvent
        {
            type = "Player_Spawn",
            t = GetTime(),
            spawn_point_id = spawnPointId
        });
        Debug.Log("[Telemetría] Player_Spawn");
    }

    public void RegisterPlayerDeath(float x, float z, string killerId)
    {
        events.Add(new TelemetryEvent
        {
            type = "Player_Death",
            t = GetTime(),
            pos_x = x,
            pos_z = z,
            killer_id = killerId
        });
        Debug.Log("[Telemetría] Player_Death por " + killerId);
    }

    public void RegisterAIDeath(float x, float z, string killerId)
    {
        events.Add(new TelemetryEvent
        {
            type = "AI_Death",
            t = GetTime(),
            pos_x = x,
            pos_z = z,
            killer_id = killerId
        });
        Debug.Log("[Telemetría] AI_Death");
    }

    public void RegisterShotFired(string weapon)
    {
        events.Add(new TelemetryEvent
        {
            type = "Shot_Fired",
            t = GetTime(),
            weapon_used = weapon
        });
    }

    public void RegisterShotHit(string hitZone)
    {
        events.Add(new TelemetryEvent
        {
            type = "Shot_Hit",
            t = GetTime(),
            hit_zone = hitZone
        });
    }

    public void RegisterItemPicked(string itemType)
    {
        events.Add(new TelemetryEvent
        {
            type = "Item_Picked",
            t = GetTime(),
            item_type = itemType
        });
        Debug.Log("[Telemetría] Item_Picked: " + itemType);
    }

    // ---- Internos ----

    private float GetTime() => Time.time - sessionStartTime;

    private IEnumerator PositionHeartbeat()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                Vector3 pos = player.transform.position;
                events.Add(new TelemetryEvent
                {
                    type = "Player_Position_Heartbeat",
                    t = GetTime(),
                    pos_x = pos.x,
                    pos_z = pos.z
                });
                Debug.Log("[Telemetría] Heartbeat registrado");
            }
        }
    }

    public string GetSessionJSON()
    {
        SessionData session = new SessionData
        {
            player_id = playerId,
            match_id = sessionId,
            total_events = events.Count,
            events = events
        };
        return JsonUtility.ToJson(session, true);
    }
}

// ---- Clases de datos serializables ----

[Serializable]
public class TelemetryEvent
{
    public string type = "";
    public float t = 0f;
    public float pos_x = 0f;
    public float pos_z = 0f;
    public string spawn_point_id = "";
    public string killer_id = "";
    public string weapon_used = "";
    public string hit_zone = "";
    public string item_type = "";
}

[Serializable]
public class SessionData
{
    public string player_id;
    public string match_id;
    public int total_events;
    public List<TelemetryEvent> events;
}