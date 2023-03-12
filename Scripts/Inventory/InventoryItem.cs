using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct InventoryItem
{
    public int ID;
    public ulong SteamID;
    public string Name;
    public string Description;
    public InventoryItemType Type;
    public Sprite Icon;
    public GameObject Prefab;

    public InventoryItem(int id, ulong steamID, string name, string description, InventoryItemType type, Sprite icon, GameObject prefab)
    {
        ID = id;
        SteamID = steamID;
        Name = name;
        Description = description;
        Type = type;
        Icon = icon;
        Prefab = prefab;
    }
}
