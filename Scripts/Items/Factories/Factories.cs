using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public sealed class Factories : MonoBehaviour
{
    [SerializeField] private RunProgress _runProgress;
    [SerializeField] private ThirdPersonController _player;
    [SerializeField] private ActiveItemsUI _activeItemsUI;
    
    private Dictionary<ItemType, Factory<Item>> _itemFactories;

    public IReadOnlyDictionary<ItemType, Factory<Item>> ItemFactories => _itemFactories;

    private void Awake()
    {  
        _itemFactories = new Dictionary<ItemType, Factory<Item>>
        {
            [ItemType.Money] = new MoneyFactory<Item>(_runProgress, true, false),
            [ItemType.Magnet] = new MagnetFactory<Item>(_activeItemsUI),
            [ItemType.HighJump] = new HighJumpFactory<Item>(_player, _activeItemsUI),
            [ItemType.RandomBoost] = new RandomItemFactory<Item>(this),
            [ItemType.DoubleMoney] = new DoubleMoneyFactory<Item>(_runProgress, _activeItemsUI),
            [ItemType.Immune] = new ImmuneFactory<Item>(_activeItemsUI),
            [ItemType.Fly] = new FlyFactory<Item>(_activeItemsUI),
            [ItemType.DoubleScore] = new DoubleScoreFactory<Item>(_runProgress,_activeItemsUI),
        };
    }
}
