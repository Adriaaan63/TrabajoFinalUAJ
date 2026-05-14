using UnityEngine;

// --------------------------------------------------------
// EVENTOS DEL PROYECTO FPS - DEATHMATCH
// --------------------------------------------------------

/// <summary>
/// Cuando el jugador reaparece en el mapa.
/// </summary>
[System.Serializable]
public class Player_Spawn : TrackerEvent
{
    public string spawn_point_id;

    public Player_Spawn(string spawnPointId) : base()
    {
        this.spawn_point_id = spawnPointId;
    }
}

/// <summary>
/// Cuando el jugador muere.
/// </summary>
[System.Serializable]
public class Player_Death : TrackerEvent
{
    public float pos_x;
    public float pos_z;
    public string killer_id;

    public Player_Death(float x, float z, string killerId) : base()
    {
        this.pos_x = x;
        this.pos_z = z;
        this.killer_id = killerId;
    }
}

/// <summary>
/// Cuando una IA muere.
/// </summary>
[System.Serializable]
public class AI_Death : TrackerEvent
{
    public float pos_x;
    public float pos_z;
    public string killer_id;

    public AI_Death(float x, float z, string killerId) : base()
    {
        this.pos_x = x;
        this.pos_z = z;
        this.killer_id = killerId;
    }
}

/// <summary>
/// Cada vez que el jugador dispara.
/// </summary>
[System.Serializable]
public class Shot_Fired : TrackerEvent
{
    public string weapon_used;

    public Shot_Fired(string weaponName) : base()
    {
        this.weapon_used = weaponName;
    }
}

/// <summary>
/// Cuando un disparo del jugador impacta en un enemigo.
/// </summary>
[System.Serializable]
public class Shot_Hit : TrackerEvent
{
    public string hit_zone; // "Head" o "Body"

    public Shot_Hit(string zone) : base()
    {
        this.hit_zone = zone;
    }
}

/// <summary>
/// Posición del jugador cada 5 segundos.
/// </summary>
[System.Serializable]
public class Player_Position_Heartbeat : TrackerEvent
{
    public float pos_x;
    public float pos_z;

    public Player_Position_Heartbeat(float x, float z) : base()
    {
        this.pos_x = x;
        this.pos_z = z;
    }
}

/// <summary>
/// Cuando el jugador recoge un objeto del mapa.
/// </summary>
[System.Serializable]
public class Item_Picked : TrackerEvent
{
    public string item_type; // "Health", "Weapon" o "Ammo"

    public Item_Picked(string type) : base()
    {
        this.item_type = type;
    }
}