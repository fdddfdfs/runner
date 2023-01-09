using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public sealed class FactoryPool<T> : Pool where T: MonoBehaviour
{
    private readonly AbstractFactory<T> _factory;
    private readonly Transform _parent;
    
    public FactoryPool(AbstractFactory<T> factory, Transform parent = null, bool isExpandable = false) :
        base(isExpandable)
    {
        _factory = factory;
        _parent = parent;
    }

    protected override GameObject AddItem(bool isActive = false)
    {
        GameObject newItem = _factory.CreateItem().gameObject;
        newItem.transform.parent = _parent;
        newItem.SetActive(isActive);
        return newItem;
    }
}
