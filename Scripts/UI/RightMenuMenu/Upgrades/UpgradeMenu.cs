using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public sealed class UpgradeMenu : Menu
{
    private const string UpgradeItemResourceName = "UI/Upgrades/UpgradeItem";
    
    [SerializeField] private RectTransform _parent;
    [SerializeField] private List<ItemData> _itemsData;
    [SerializeField] private ScrollRect _scrollRect;
    
    private Dictionary<ItemType, (Func<int> getLevel, Action increaseLevel)> _upgradeActions;
    private UpgradeItem _firstUpgradeItem;

    public override void ChangeMenuActive(bool active)
    {
        base.ChangeMenuActive(active);

        if (active && _firstUpgradeItem)
        {
            _firstUpgradeItem.SelectButton();
        }
    }

    private void Start()
    {
        GameObject upgradeItemPrefab = ResourcesLoader.LoadObject(UpgradeItemResourceName);
        
        _upgradeActions = new Dictionary<ItemType, (Func<int> getLevel, Action increaseLevel)>
        {
            { ItemType.Fly, (() => Stats.FlyLevel.Value, () => Stats.FlyLevel.Value += 1) },
            { ItemType.Immune, (() => Stats.ImmuneLevel.Value, () => Stats.ImmuneLevel.Value += 1) },
            { ItemType.Magnet, (() => Stats.MagnetLevel.Value, () => Stats.MagnetLevel.Value += 1) },
            { ItemType.DoubleMoney, (() => Stats.DoubleMoneyLevel.Value, () => Stats.DoubleMoneyLevel.Value += 1) },
            { ItemType.HighJump, (() => Stats.HighJumpLevel.Value, () => Stats.HighJumpLevel.Value += 1) },
            { ItemType.DoubleScore, (() => Stats.DoubleScoreLevel.Value, () => Stats.DoubleScoreLevel.Value += 1) },
            { ItemType.Board, (() => Stats.BoardLevel.Value, () => Stats.BoardLevel.Value += 1) },
        };

        var parentVerticalLayoutGroup = _parent.GetComponent<VerticalLayoutGroup>();
        _parent.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Vertical,
            (upgradeItemPrefab.GetComponent<RectTransform>().rect.height +
             parentVerticalLayoutGroup.spacing) *
            _itemsData.Count +
            parentVerticalLayoutGroup.padding.bottom +
            parentVerticalLayoutGroup.padding.top);

        for (int i = 0; i < _itemsData.Count; i++)
        {
            if (!_upgradeActions.ContainsKey(_itemsData[i].Type))
            {
                throw new Exception($"Upgrade actions not implemented for type {_itemsData[i].Type}");
            }

            GameObject newUpgrade = Instantiate(upgradeItemPrefab, _parent);
            var upgradeItem = newUpgrade.GetComponent<UpgradeItem>();
            upgradeItem.Init(
                _itemsData[i].Icon,
                _itemsData[i].Name,
                _itemsData[i].Description,
                _upgradeActions[_itemsData[i].Type].getLevel,
                _upgradeActions[_itemsData[i].Type].increaseLevel,
                i);
            
            upgradeItem.OnSelect += SetVerticalPosition;

            if (i == 0)
            {
                _firstUpgradeItem = upgradeItem;
                _firstUpgradeItem.SelectButton();
            }
        }
    }

    private void SetVerticalPosition(int index)
    {
        if (Gamepad.current == null) return;
        
        _scrollRect.verticalScrollbar.value = 1 - (float)index / (_itemsData.Count - 1);
    }
}