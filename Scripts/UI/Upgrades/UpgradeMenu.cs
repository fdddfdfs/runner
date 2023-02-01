using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public sealed class UpgradeMenu : MonoBehaviour
{
    [SerializeField] private GameObject _menu;
    [SerializeField] private RectTransform _parent;
    [SerializeField] private GameObject _upgradeItemPrefab;
    [SerializeField] private List<ItemType> _itemTypes;
    [SerializeField] private List<Sprite> _itemSprites;
    [SerializeField] private ScrollRect _scrollRect;
    
    private Dictionary<ItemType, (Func<int> getLevel, Action increaseLevel)> _upgradeActions;
    private UpgradeItem _firstUpgradeItem;

    public void ChangeMenuVisible(bool visible)
    {
        _menu.SetActive(visible);

        if (visible && _firstUpgradeItem)
        {
            _firstUpgradeItem.SelectButton();
        }
    }

    private void Start()
    {
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

        Dictionary<ItemType, (string name, string description)> itemInfo = new()
        {
            [ItemType.Fly] = ("Plane", "Allow you to fly"),
            [ItemType.Immune] = ("Immune", "Makes you invulnerable"),
            [ItemType.Magnet] = ("Magnet", "Magnet for coins"),
            [ItemType.DoubleMoney] = ("DoubleMoney", "Increase your money gain by 2 times"),
            [ItemType.HighJump] = ("HighJump", "Makes you jump higher"),
            [ItemType.DoubleScore] = ("DoubleScore", "Increase your score gain by 2 times"),
            [ItemType.Board] = ("Board", "Allow you to take one hit"),
        };

        var parentVerticalLayoutGroup = _parent.GetComponent<VerticalLayoutGroup>();
        _parent.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Vertical,
            (_upgradeItemPrefab.GetComponent<RectTransform>().rect.height +
             parentVerticalLayoutGroup.spacing) *
            _itemTypes.Count +
            parentVerticalLayoutGroup.padding.bottom +
            parentVerticalLayoutGroup.padding.top);

        for (int i = 0; i < _itemTypes.Count; i++)
        {
            if (!_upgradeActions.ContainsKey(_itemTypes[i]))
            {
                throw new Exception($"Upgrade actions not implemented for type {_itemTypes[i]}");
            }

            GameObject newUpgrade = Instantiate(_upgradeItemPrefab, _parent);
            var upgradeItem = newUpgrade.GetComponent<UpgradeItem>();
            upgradeItem.Init(
                _itemSprites[i],
                itemInfo[_itemTypes[i]].name,
                itemInfo[_itemTypes[i]].description,
                _upgradeActions[_itemTypes[i]].getLevel,
                _upgradeActions[_itemTypes[i]].increaseLevel,
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
        _scrollRect.verticalScrollbar.value = 1 - (float)index / (_itemTypes.Count - 1);
    }
}