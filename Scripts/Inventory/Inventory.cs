using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class Inventory : Menu
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

    public void Init(IClothesChanger clothesChanger)
    {
        _clothesChanger = clothesChanger;
    }
    
    public void OpenInventory(Type inventoryType)
    {
        OpenNewInventory(_inventories[inventoryType]);
    }

    private void Awake()
    {
        var inventorySteamworks = new InventorySteamworks();
         var inventoryChests = new InventoryChests(
            inventorySteamworks,
            _inventoryCells,
            _nameText,
            _descriptionText,
            _button,
            _nextPageButton,
            _previousPageButton,
            _chest,
            _invisibleSprite);
         var inventoryClothes = new InventoryClothes(
             inventorySteamworks,
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
