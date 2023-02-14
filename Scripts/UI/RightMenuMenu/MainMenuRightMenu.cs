using System.Collections.Generic;
using System.ComponentModel;
using fdddfdfs.Leaderboard;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuRightMenu : Menu
{
    private const string UpgradeMenuResourceName = "UI/Upgrades/UpgradeMenu";
    private const string SettingsMenuResourceName = "UI/Settings/SettingsMenu";
    private const string LeaderboardMenuResourceName = "UI/Leaderboard/LeaderboardMenu";

    private const string LeaderboardName = "Leaders";

    [SerializeField] private List<Button> _submenuButtons;
    [SerializeField] private Transform _submenuParent;

    private List<Menu> _submenus;
    private Menu _currentSubmenu;

    public LeaderboardController LeaderboardController {get; private set;}

    public override void ChangeMenuActive(bool active)
    {
        base.ChangeMenuActive(active);

        if (_currentSubmenu)
        {
            _currentSubmenu.ChangeMenuActive(active);
        }
    }

    private void Awake()
    {
        LeaderboardController = SpawnMenu<LeaderboardController>(LeaderboardMenuResourceName);
        LeaderboardController.SetCurrentLeaderboard(LeaderboardName);
        
        _submenus = new List<Menu>
        {
            SpawnMenu<UpgradeMenu>(UpgradeMenuResourceName),
            SpawnMenu<SettingsMenu>(SettingsMenuResourceName),
            LeaderboardController,
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