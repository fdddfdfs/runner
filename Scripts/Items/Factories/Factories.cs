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
        _itemFactories = new Dictionary<ItemType, ItemFactory<Item>>
        {
            [ItemType.Money] = new MoneyItemFactory<Item>(_runProgress, true, false),
            [ItemType.Magnet] = new MagnetItemFactory<Item>(_activeItemsUI),
            [ItemType.HighJump] = new HighJumpItemFactory<Item>(_player, _activeItemsUI),
            [ItemType.RandomBoost] = new RandomItemItemFactory<Item>(this),
            [ItemType.DoubleMoney] = new DoubleMoneyItemFactory<Item>(_runProgress, _activeItemsUI),
            [ItemType.Immune] = new ImmuneItemFactory<Item>(_activeItemsUI),
            [ItemType.Fly] = new FlyItemFactory<Item>(_activeItemsUI),
            [ItemType.DoubleScore] = new DoubleScoreItemFactory<Item>(_runProgress,_activeItemsUI),
            [ItemType.Spring] = new SpringItemFactory<Item>(),
        };
    }
}
