using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class UpgradeMenu : MonoBehaviour
{
    [SerializeField] private GameObject _menu;
    [SerializeField] private Transform _parent;
    [SerializeField] private GameObject _upgradeItemPrefab;
    [SerializeField] private List<ItemType> _itemTypes;
    [SerializeField] private List<Sprite> _itemSprites;
    
    private Dictionary<ItemType, (Func<int> getLevel, Action increaseLevel)> _upgradeActions;

    public void ChangeMenuVisible(bool visible)
    {
        _menu.SetActive(visible);
    }
    
    private void Start()
    {
        _upgradeActions = new Dictionary<ItemType, (Func<int> getLevel, Action increaseLevel)>
        {
            { ItemType.Fly, (()=> Stats.FlyLevel.Value, ()=> Stats.FlyLevel.Value += 1) },
            { ItemType.Immune, (()=> Stats.ImmuneLevel.Value, ()=> Stats.ImmuneLevel.Value += 1) },
            { ItemType.Magnet, (()=> Stats.MagnetLevel.Value, ()=> Stats.MagnetLevel.Value += 1) },
            { ItemType.DoubleMoney, (()=> Stats.DoubleMoneyLevel.Value, ()=> Stats.DoubleMoneyLevel.Value += 1) },
            { ItemType.HighJump, (()=> Stats.HighJumpLevel.Value, ()=> Stats.HighJumpLevel.Value += 1) },
            { ItemType.DoubleScore, (()=> Stats.DoubleScoreLevel.Value, ()=> Stats.DoubleScoreLevel.Value += 1) },
            { ItemType.Board, (()=> Stats.BoardLevel.Value, ()=> Stats.BoardLevel.Value += 1) },
        };

        for (int i = 0; i < _itemTypes.Count; i++)
        {
            if (!_upgradeActions.ContainsKey(_itemTypes[i]))
            {
                throw new Exception($"Upgrade actions not implemented for type {_itemTypes[i]}");
            }

            GameObject newUpgrade = Instantiate(_upgradeItemPrefab, _parent);
            UpgradeItem upgradeItem = newUpgrade.GetComponent<UpgradeItem>();
            upgradeItem.Init(
                _itemSprites[i],
                "item",
                "desc",
                _upgradeActions[_itemTypes[i]].getLevel,
                _upgradeActions[_itemTypes[i]].increaseLevel);
        }

    }
}