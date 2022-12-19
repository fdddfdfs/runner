using System.Collections.Generic;
using UnityEngine;

public class Obstacle: MonoBehaviour
{
    [SerializeField] protected List<ItemParent> _items;

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

    public virtual void HideObstacle()
    {
        foreach (var item in _items)
        {
            item.HideObstacle();
        }
        
        gameObject.SetActive(false);
    }

    public virtual void EnterObstacle()
    {
        foreach (var item in _items)
        {
            if (!item.ItemObject.gameObject.activeSelf)
            {
                item.ItemObject.gameObject.SetActive(true);
            }
            
            item.EnterObstacle();
        }
    }
}