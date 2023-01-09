using UnityEngine;

public sealed class FactoryPoolMono<T> : PoolMono<T> where T: MonoBehaviour
{
    private readonly AbstractFactory<T> _factory;
    private readonly Transform _parent;
    
    public FactoryPoolMono(AbstractFactory<T> factory, Transform parent = null, bool isExpandable = false) :
        base(isExpandable)
    {
        _factory = factory;
        _parent = parent;
    }

    protected override T AddItem(bool isActive = false)
    {
        T newItem = _factory.CreateItem();
        newItem.transform.parent = _parent;
        newItem.gameObject.SetActive(isActive);
        _pool.Add(newItem);

        return newItem;
    }
}