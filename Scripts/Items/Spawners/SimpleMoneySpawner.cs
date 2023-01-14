using System.Collections.Generic;
using UnityEngine;

public static class SimpleMoneySpawner
{
    private const float MoneyDistance = 1f;

    public static List<ItemParent> SpawnMoneys(Transform parent, Vector3 startPosition, float sizeZ)
    {
        List<ItemParent> items = new();
        float currentPositionZ = 0;
        while (currentPositionZ < sizeZ)
        {
            var gameObject = new GameObject
            {
                transform =
                {
                    parent = parent,
                    localPosition = startPosition + Vector3.forward * currentPositionZ,
                }
            };

            var item = gameObject.AddComponent<RuntimeItemParent>();
            item.Init(ItemType.Money);
            items.Add(item);
            
            currentPositionZ += MoneyDistance;
        }

        return items;
    }
}