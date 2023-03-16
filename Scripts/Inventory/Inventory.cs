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
    [SerializeField] private Button _openChests;

    [SerializeField] private GameObject _chest;

    [SerializeField] private Sprite _invisibleSprite;
    
    private Dictionary<Type, InventoryGrid> _inventories;
    private InventoryGrid _currentInventoryGrid;
    private IClothesChanger _clothesChanger;
    private InventorySteamworks _inventorySteamworks;

    public void Init(IClothesChanger clothesChanger)
    {
        _clothesChanger = clothesChanger;
    }
    
    public void OpenInventory(Type inventoryType)
    {
        OpenNewInventory(_inventories[inventoryType]);
    }

    public void SetInventoryResult(SteamInventoryResult_t inventoryResult)
    {
        _inventorySteamworks.SetInventoryResult(inventoryResult);
    }

    private void Awake()
    {
         _inventorySteamworks = new InventorySteamworks();
         var inventoryChests = new InventoryChests(
             _inventorySteamworks,
            _inventoryCells,
            _nameText,
            _descriptionText,
            _button,
            _nextPageButton,
            _previousPageButton,
             _invisibleSprite);
         var inventoryClothes = new InventoryClothes(
             _inventorySteamworks,
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

    private void OpenNewInventory(InventoryGrid newInventory)
    {
        if (_currentInventoryGrid == newInventory)
            return;

        _currentInventoryGrid?.ClearItems();

        newInventory.ShowItems();
        _currentInventoryGrid = newInventory;
    }
}
