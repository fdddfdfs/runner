using System.Collections.Generic;
using UnityEngine;

public class InventoryAllItems : MonoBehaviour
{
    public const int PlaytimeGeneratorID = 1000;
    
    [SerializeField] private List<InventoryItemData> _inventoryItemsData;

    private static InventoryAllItems _instance;

    public Dictionary<int, InventoryItemData> Items { get; private set; }

    public static InventoryAllItems Instance => _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        Items = new Dictionary<int, InventoryItemData>();

        foreach (InventoryItemData inventoryItemData in _inventoryItemsData)
        {
            Items.Add(inventoryItemData.ID, inventoryItemData);
        }
    }
}
