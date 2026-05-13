using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelemetryTracker : MonoBehaviour
{
    public static TelemetryTracker Instance;

    // Lista guardado eventos
    private List<Dictionary<string, object>> events = new List<Dictionary<string, object>>();

    // Datos de sesion
    private string sessionId;
    private string playerId = "Jugador_01"; 
    private float sessionStartTime;

    void Awake()
    {
      
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Generar ID
        sessionId = Guid.NewGuid().ToString();
        sessionStartTime = Time.time;

        // Iniciar  heartbeat
        StartCoroutine(PositionHeartbeat());

        Debug.Log("Telemetría iniciada. Sesión: " + sessionId);
    }


    public void RegisterSpawn(string spawnPointId)
    {
        AddEvent("Player_Spawn", new Dictionary<string, object>
        {
            { "spawn_point_id", spawnPointId }
        });
    }

    public void RegisterPlayerDeath(float posX, float posZ, string killerId)
    {
        AddEvent("Player_Death", new Dictionary<string, object>
        {
            { "pos_x", posX },
            { "pos_z", posZ },
            { "killer_id", killerId }
        });
    }

    public void RegisterAIDeath(float posX, float posZ, string killerId)
    {
        AddEvent("AI_Death", new Dictionary<string, object>
        {
            { "pos_x", posX },
            { "pos_z", posZ },
            { "killer_id", killerId }
        });
    }

    public void RegisterShotFired(string weaponName)
    {
        AddEvent("Shot_Fired", new Dictionary<string, object>
        {
            { "weapon_used", weaponName }
        });
    }

    public void RegisterShotHit(string hitZone) // "Body" o "Head"
    {
        AddEvent("Shot_Hit", new Dictionary<string, object>
        {
            { "hit_zone", hitZone }
        });
    }

    public void RegisterItemPicked(string itemType) // "Health", "Weapon", "Ammo"
    {
        AddEvent("Item_Picked", new Dictionary<string, object>
        {
            { "item_type", itemType }
        });
    }


    private void AddEvent(string eventType, Dictionary<string, object> attributes)
    {
        var newEvent = new Dictionary<string, object>
        {
            { "type", eventType },
            // Tiempo sesión
            { "t", Time.time - sessionStartTime }
        };

        
        foreach (var attr in attributes)
            newEvent[attr.Key] = attr.Value;

        events.Add(newEvent);
        Debug.Log($"[Telemetría] Evento registrado: {eventType}");
    }

    private IEnumerator PositionHeartbeat()
    {
        
        GameObject player = GameObject.FindWithTag("Player");

        while (true)
        {
            yield return new WaitForSeconds(5f);

            if (player != null)
            {
                Vector3 pos = player.transform.position;
                AddEvent("Player_Position_Heartbeat", new Dictionary<string, object>
                {
                    { "pos_x", pos.x },
                    { "pos_z", pos.z }
                });
            }
        }
    }

    // Devuelve el JSON
    public string GetSessionJSON()
    {
        
        var session = new Dictionary<string, object>
        {
            { "player_id", playerId },
            { "match_id", sessionId },
            { "total_events", events.Count },
            { "events", events }
        };

        return JsonUtility.ToJson(new SerializableSession(session));
    }
}

[Serializable]
public class SerializableEvent
{
    public string type;
    public float t;
    public float pos_x;
    public float pos_z;
    public string spawn_point_id;
    public string killer_id;
    public string weapon_used;
    public string hit_zone;
    public string item_type;
}

[Serializable]
public class SerializableSession
{
    public string player_id;
    public string match_id;
    public int total_events;
    public List<SerializableEvent> events;

    public SerializableSession(Dictionary<string, object> data) { /* conversión */ }
}