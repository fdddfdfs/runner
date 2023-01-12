using System;
using System.Collections.Generic;
using System.Linq;

public class WeightedRandomItemFactory<T> : ItemFactory<T> where T: Item
{
    private readonly WeightRandom _weightRandom;
    private readonly Factories _factories;
    private readonly List<ItemType> _itemTypes;

    protected override string PrefabName => null;

    public WeightedRandomItemFactory(Factories factories ,Dictionary<ItemType, int> weights)
    {
        _factories = factories;
        _weightRandom = new WeightRandom(weights.Select(pair => pair.Value).ToList(), true);
        _itemTypes = weights.Select(pair => pair.Key).ToList();
    }

    public override T CreateItem()
    {
        int r = _weightRandom.GetRandom();
        return _factories.ItemFactories[_itemTypes[r]].CreateItem() as T;
    }
}