using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public sealed class Factories : MonoBehaviour
{
    [SerializeField] private RunProgress _runProgress;
    [SerializeField] private ThirdPersonController _player;
    [SerializeField] private ActiveItemsUI _activeItemsUI;
    [SerializeField] private Run _run;
    [SerializeField] private PickupCar _pickupCar;
    
    private Dictionary<ItemType, ItemFactory<Item>> _itemFactories;

    public IReadOnlyDictionary<ItemType, ItemFactory<Item>> ItemFactories => _itemFactories;

    private void Awake()
    {
        ItemsActiveStates itemsActiveStates = new(_run);
        
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
            [ItemType.Board] = BoardItem.Weight,
        };

        Effects effects = _player.Effects;
        
        _itemFactories = new Dictionary<ItemType, ItemFactory<Item>>
        {
            [ItemType.HighJump] = new HighJumpItemFactory<Item>(
                _player,
                _activeItemsUI,
                _run,
                itemsActiveStates,
                effects),
            [ItemType.DoubleMoney] = new DoubleMoneyItemFactory<Item>(
                _runProgress,
                _activeItemsUI,
                _run,
                itemsActiveStates,
                effects),
            [ItemType.DoubleScore] = new DoubleScoreItemFactory<Item>(
                _runProgress,
                _activeItemsUI,
                _run,
                itemsActiveStates,
                effects),
            [ItemType.Money] = new MoneyItemFactory<Item>(
                _runProgress,
                _run,
                false,
                false,
                effects),
            [ItemType.Magnet] = new MagnetItemFactory<Item>(_activeItemsUI, _run, itemsActiveStates, effects),
            [ItemType.Immune] = new ImmuneItemFactory<Item>(_activeItemsUI, _run, itemsActiveStates, effects),
            [ItemType.Fly] = new FlyItemFactory<Item>(_activeItemsUI, _run, itemsActiveStates, effects),
            [ItemType.Spring] = new SpringItemFactory<Item>(_run,  effects),
            [ItemType.Board] = new BoardItemFactory<Item>(_run, effects, _pickupCar),
        };
        
        _itemFactories.Add(ItemType.RandomBoost, new RandomItemFactory<Item>(this));
        _itemFactories.Add(ItemType.WeightedRandomBoost, new WeightedRandomItemFactory<Item>(this, weights));
    }
}
