using System;
using System.Collections.Generic;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class Inventory : Menu
{
    [SerializeField] private List<GameObject> _inventoryCells;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private Button _button;
    [SerializeField] private Button _nextPageButton, _previousPageButton;

    [SerializeField] private Sprite _invisibleSprite;
    
    private Dictionary<Type, InventoryGrid> _inventories;
    private InventoryGrid _currentInventoryGrid;
    private IClothesChanger _clothesChanger;

    public InventorySteamworks InventorySteamworks { get; private set; }

    public void Init(IClothesChanger clothesChanger)
    {
        _clothesChanger = clothesChanger;
        
        InventorySteamworks = new InventorySteamworks();
        var inventoryChests = new InventoryChests(
            InventorySteamworks,
            _inventoryCells,
            _nameText,
            _descriptionText,
            _button,
            _nextPageButton,
            _previousPageButton,
            _invisibleSprite);
        var inventoryClothes = new InventoryClothes(
            InventorySteamworks,
            _inventoryCells,
            _nameText,
            _descriptionText,
            _button,
            _nextPageButton,
            _previousPageButton,
            _clothesChanger,
            _invisibleSprite);

        _inventories = new Dictionary<Type, InventoryGrid>
        {
            [typeof(InventoryClothes)] = inventoryClothes,
            [typeof(InventoryChests)] = inventoryChests,
        };
    }
    
    public void OpenInventory(Type inventoryType)
    {
        OpenNewInventory(_inventories[inventoryType]);
    }

    public void SetInventoryResult(SteamInventoryResult_t inventoryResult)
    {
        InventorySteamworks.SetInventoryResult(inventoryResult);
    }

    private void OpenNewInventory(InventoryGrid newInventory)
    {
        if (_currentInventoryGrid == newInventory)
            return;

        _currentInventoryGrid?.ClearItems();

        newInventory.ShowItems();
        _currentInventoryGrid = newInventory;
    }

    private void OnDestroy()
    {
        foreach (KeyValuePair<Type,InventoryGrid> inventoryGrid in _inventories)
        {
            inventoryGrid.Value.ClearEvents();
        }
    }
}
