using System.Collections.Generic;
using System.Linq;

public sealed class RandomItemFactory<T> : ItemFactory<T> where T: Item
{
    protected override string PrefabName => null;

    private readonly List<ItemFactory<Item>> _factories;
    
    public RandomItemFactory(Factories factories)
    {
        _factories = factories.ItemFactories
            .Where(pair => (pair.Key & (ItemType.RandomBoost | ItemType.WeightedRandomBoost | ItemType.Money)) == 0)
            .Select(pair => pair.Value).ToList();
    }

    public override T CreateItem()
    {
        int r = UnityEngine.Random.Range(0, _factories.Count);
        return _factories[r].CreateItem() as T;
    }
}