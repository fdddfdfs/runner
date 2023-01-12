using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public sealed class Factories : MonoBehaviour
{
    [SerializeField] private RunProgress _runProgress;
    [SerializeField] private ThirdPersonController _player;
    [SerializeField] private ActiveItemsUI _activeItemsUI;
    
    private Dictionary<ItemType, ItemFactory<Item>> _itemFactories;

    public IReadOnlyDictionary<ItemType, ItemFactory<Item>> ItemFactories => _itemFactories;

    private void Awake()
    {
        Dictionary<ItemType, int> weights = new()
        {
            [ItemType.Money] = Money.Weight,
            [ItemType.Magnet] = Magnet.Weight,
            [ItemType.HighJump] = HighJump.Weight,
            [ItemType.DoubleMoney] = DoubleMoney.Weight,
            [ItemType.Immune] = Immune.Weight,
            [ItemType.Fly] = Fly.Weight,
            [ItemType.DoubleScore] = DoubleScore.Weight,
            [ItemType.Spring] = Spring.Weight,
        };
        
        _itemFactories = new Dictionary<ItemType, ItemFactory<Item>>
        {
            [ItemType.Money] = new MoneyItemFactory<Item>(_runProgress, true, false),
            [ItemType.Magnet] = new MagnetItemFactory<Item>(_activeItemsUI),
            [ItemType.HighJump] = new HighJumpItemFactory<Item>(_player, _activeItemsUI),
            [ItemType.DoubleMoney] = new DoubleMoneyItemFactory<Item>(_runProgress, _activeItemsUI),
            [ItemType.Immune] = new ImmuneItemFactory<Item>(_activeItemsUI),
            [ItemType.Fly] = new FlyItemFactory<Item>(_activeItemsUI),
            [ItemType.DoubleScore] = new DoubleScoreItemFactory<Item>(_runProgress,_activeItemsUI),
            [ItemType.Spring] = new SpringItemFactory<Item>(),
        };
        
        _itemFactories.Add(ItemType.RandomBoost, new RandomItemFactory<Item>(this));
        _itemFactories.Add(ItemType.WeightedRandomBoost, new WeightedRandomItemFactory<Item>(this, weights));
    }
}
