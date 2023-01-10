using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoolMono<T> where T: MonoBehaviour
{ 
    protected const int DefaultStartCount = 20;

    private readonly bool _isExpandable;
    private readonly int _startCount;
    private readonly int _poolSize;
    
    protected bool _isInitialized;
    protected List<T> _pool;

    protected PoolMono(bool isExpandable = false, int startCount = DefaultStartCount)
    {
        _pool = new List<T>();
        _isExpandable = isExpandable;
        _poolSize = startCount;
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