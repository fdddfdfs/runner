using System;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMenu : MonoBehaviour
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
            { ItemType.Fly , (()=> Stats.Instance.FlyLevel, ()=> Stats.Instance.FlyLevel += 1) },
            { ItemType.Immune , (()=> Stats.Instance.ImmuneLevel, ()=> Stats.Instance.ImmuneLevel += 1) },
            { ItemType.Magnet , (()=> Stats.Instance.MagnetLevel, ()=> Stats.Instance.MagnetLevel += 1) },
            { ItemType.DoubleMoney , (()=> Stats.Instance.DoubleMoneyLevel, ()=> Stats.Instance.DoubleMoneyLevel += 1) },
            { ItemType.HighJump , (()=> Stats.Instance.HighJumpLevel, ()=> Stats.Instance.HighJumpLevel += 1) },
            { ItemType.DoubleScore, (()=> Stats.Instance.DoubleScoreLevel, ()=> Stats.Instance.DoubleScoreLevel += 1) },
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