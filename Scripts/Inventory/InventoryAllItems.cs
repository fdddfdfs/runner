using System.Collections.Generic;
using UnityEngine;

public sealed class InventoryAllItems : MonoBehaviour
{
    public const int PlaytimeGeneratorID = 1000;
    
    [SerializeField] private List<InventoryItemData> _inventoryItemsData;

    public Dictionary<int, InventoryItemData> Items { get; private set; }

    public static InventoryAllItems Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Items = new Dictionary<int, InventoryItemData>();

        foreach (InventoryItemData inventoryItemData in _inventoryItemsData)
        {
            Items.Add(inventoryItemData.ID, inventoryItemData);
        }
    }
}
