﻿using System.Collections.Generic;
using System.ComponentModel;
using fdddfdfs.Leaderboard;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuRightMenu : Menu
{
    private const string UpgradeMenuResourceName = "UI/Upgrades/UpgradeMenu";
    private const string SettingsMenuResourceName = "UI/Settings/SettingsMenu";
    private const string LeaderboardMenuResourceName = "UI/Leaderboard/LeaderboardMenu";
    private const string InventoryMenuResourceName = "UI/Inventory/Inventory";

    private const string LeaderboardName = "Leaders";

    [SerializeField] private List<Button> _submenuButtons;
    [SerializeField] private Transform _submenuParent;
    [SerializeField] private Cutscenes _cutscenes;

    private List<Menu> _submenus;
    private Menu _currentSubmenu;

    private Inventory _inventory;

    public LeaderboardController LeaderboardController {get; private set;}

    public override void ChangeMenuActive(bool active)
    {
        base.ChangeMenuActive(active);

        if (_currentSubmenu)
        {
            _currentSubmenu.ChangeMenuActive(active);
        }
    }

    public void SetInventoryResult(SteamInventoryResult_t inventoryResult)
    {
        _inventory.SetInventoryResult(inventoryResult);
    }

    private void Start()
    {
        LeaderboardController = SpawnMenu<LeaderboardController>(LeaderboardMenuResourceName);
        LeaderboardController.SetCurrentLeaderboard(LeaderboardName);

        _inventory = SpawnMenu<Inventory>(InventoryMenuResourceName);
        _inventory.Init(_cutscenes);
        
        _submenus = new List<Menu>
        {
            SpawnMenu<UpgradeMenu>(UpgradeMenuResourceName),
            SpawnMenu<SettingsMenu>(SettingsMenuResourceName),
            LeaderboardController,
            _inventory,
            _inventory,
        };

        if (_submenuButtons.Count != _submenus.Count)
        {
            throw new WarningException($"Menu count: {_submenus.Count} != Button count: {_submenuButtons.Count}");
        }

        for (var i = 0; i < _submenus.Count; i++)
        {
            int index = i;
            _submenuButtons[i].onClick.AddListener(() => ChangeCurrentSubmenu(index));
        }
        
        _submenuButtons[3].onClick.AddListener(() => _inventory.OpenInventory(typeof(InventoryClothes)));
        _submenuButtons[4].onClick.AddListener(() => _inventory.OpenInventory(typeof(InventoryChests)));

        ChangeCurrentSubmenu(0);
    }

    private T SpawnMenu<T>(string resourceName) where T: Menu
    {
        var menu = ResourcesLoader.InstantiateLoadComponent<T>(resourceName);
        menu.transform.SetParent(_submenuParent, false);
        menu.ChangeMenuActive(false);

        return menu;
    }

    private void ChangeCurrentSubmenu(int newMenu)
    {
        Menu menu = _submenus[newMenu];
        
        if (_currentSubmenu == menu) return;
        
        if (_currentSubmenu)
        {
            _currentSubmenu.ChangeMenuActive(false);
        }

        _currentSubmenu = menu;
        _currentSubmenu.ChangeMenuActive(true);
    }
}