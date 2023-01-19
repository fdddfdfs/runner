using System.Collections.Generic;
using UnityEngine;

public class Obstacle: MonoBehaviour
{
    [SerializeField] protected List<ItemParent> _items;

    private bool _needShowItems;

    public void Init(Factories factories, bool needShowItems)
    {
        _needShowItems = needShowItems;
        
        if (!_needShowItems) return;

        foreach (ItemParent item in _items)
        {
            Item createdItem = factories.ItemFactories[item.ItemType].CreateItem();
            Transform createdItemTransform = createdItem.transform;
            createdItemTransform.parent = item.transform;
            createdItemTransform.localPosition = Vector3.zero;
            item.ItemObject = createdItem;
        }
    }

    public virtual void HideObstacle()
    {
        if (!_needShowItems) return;

        foreach (ItemParent item in _items)
        {
            if (!item.ItemObject.gameObject.activeSelf)
            {
                item.ItemObject.gameObject.SetActive(true);
            }
            
            item.HideObstacle();
        }
    }

    public virtual void EnterObstacle()
    {
        if (!_needShowItems) return;
        
        foreach (ItemParent item in _items)
        {
            item.EnterObstacle();
        }
    }
}