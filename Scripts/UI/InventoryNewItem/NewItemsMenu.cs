using System;
using System.Collections.Generic;
using UnityEngine;

public class NewItemsMenu: MonoBehaviour
{
    private const string NewItemResourceName = "UI/NewItems/NewItem";
    private const float AppearTime = 3f;

    [SerializeField] private MainMenuRightMenu _mainMenuRightMenu;

    private PoolMono<NewItem> _newItemsPool;
    
    private void Awake()
    {
        _newItemsPool = new GameObjectPoolMono<NewItem>(
            ResourcesLoader.LoadObject(NewItemResourceName),
            gameObject.transform,
            true,
            1);
    }

    private void Start()
    {
        _mainMenuRightMenu.Inventory.InventorySteamworks.InventoryAddItem += ShowItem;
    }

    private void ShowItem(List<InventoryItem> addedItems)
    {
        foreach (InventoryItem addedItem in addedItems)
        {
            NewItem newItem = _newItemsPool.GetItem();
            newItem.transform.SetAsLastSibling();
            newItem.Appear(AppearTime, addedItem.InventoryItemData);
        }
    }
}