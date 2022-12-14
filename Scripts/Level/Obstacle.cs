using System.Collections.Generic;
using UnityEngine;

public class Obstacle: MonoBehaviour
{
    [SerializeField] private List<ItemParent> _items;

    public void Init(Factories factories)
    {
        foreach (var item in _items)
        {
            Item createdItem = factories.ItemFactories[item.ItemType].CreateItem();
            var createdItemTransform = createdItem.transform;
            createdItemTransform.parent = item.transform;
            createdItemTransform.localPosition = Vector3.zero;
            item.ItemObject = createdItem;
        }
    }

    public void HideObstacle()
    {
        foreach (var item in _items)
        {
            if (!item.ItemObject.gameObject.activeSelf)
            {
                item.ItemObject.gameObject.SetActive(true);
            }
        }
        
        gameObject.SetActive(false);
    }
}