using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public abstract class PoolMono<T> where T: MonoBehaviour
{ 
    protected const int DefaultStartCount = 20;
    
    protected List<T> _pool;
    
    private readonly bool _isExpandable;
    
    protected int _poolSize;
    protected bool _isInitialized;

    protected PoolMono(bool isExpandable = false)
    {
        _pool = new List<T>();
        _isExpandable = isExpandable;
    }

    public T GetItem()
    {
        if (!_isInitialized)
        {
            InitializePool();
            _isInitialized = true;
        }
        
        for (int i = 0; i < _pool.Count; i++)
        {
            if (!_pool[i].gameObject.activeSelf)
            {
                _pool[i].gameObject.SetActive(true);
                return _pool[i];
            }
        }

        if (_isExpandable)
        {
            return AddItem(true);
        }
        
        throw new Exception("Pool expend");
    }

    private void InitializePool()
    {
        for (int i = _pool.Count; i < _poolSize; i++)
        {
            AddItem();
        }
    }

    protected abstract T AddItem(bool isActive = false);
}