using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuRightMenu : Menu
{
    private const string UpgradeMenuResourceName = "UI/Upgrades/UpgradeMenu";
    private const string SettingsMenuResourceName = "UI/Settings/SettingsMenu";

    [SerializeField] private List<Button> _submenuButtons;
    [SerializeField] private Transform _submenuParent;

    private List<Menu> _submenus;
    private Menu _currentMenu;
    
    private void Awake()
    {
        _submenus = new List<Menu>
        {
            SpawnMenu<UpgradeMenu>(UpgradeMenuResourceName),
            SpawnMenu<SettingsMenu>(SettingsMenuResourceName),
        };

        if (_submenuButtons.Count != _submenus.Count)
        {
            throw new WarningException($"Menu count: {_submenus.Count} != Button count: {_submenuButtons.Count}");
        }

        for (var i = 0; i < _submenus.Count; i++)
        {
            int index = i;
            _submenuButtons[i].onClick.AddListener(() => ChangeCurrentMenu(index));
        }
        
        ChangeCurrentMenu(0);
    }

    private T SpawnMenu<T>(string resourceName) where T: MonoBehaviour
    {
        var menu = ResourcesLoader.InstantiateLoadComponent<T>(resourceName);
        menu.transform.SetParent(_submenuParent, false);

        return menu;
    }

    private void ChangeCurrentMenu(int newMenu)
    {
        Menu menu = _submenus[newMenu];
        
        if (_currentMenu == menu) return;
        
        if (_currentMenu)
        {
            _currentMenu.ChangeMenuActive(false);
        }

        _currentMenu = menu;
        _currentMenu.ChangeMenuActive(true);
    }
}