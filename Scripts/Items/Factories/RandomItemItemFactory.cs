using System;
using Unity.VisualScripting;

public class RandomItemItemFactory<T> : ItemFactory<T> where T: Item
{
    protected override string PrefabName { get; }

    private Factories _factories;
    
    public RandomItemItemFactory(Factories factories)
    {
        _factories = factories;
    }

    public override T CreateItem()
    {
        int n = 0;
        int r = UnityEngine.Random.Range(0, _factories.ItemFactories.Count);
        while ( n < _factories.ItemFactories.Count * 2)
        {
            foreach (var factory in _factories.ItemFactories)
            {
                if (n > r && factory.Key != ItemType.RandomBoost)
                {
                    return factory.Value.CreateItem() as T;
                }

                n++;
            }
        }

        throw new Exception("Cannot find random item");
    }
}