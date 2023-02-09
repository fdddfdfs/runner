using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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

        T item = FindFreeElement(0, _pool.Count);
        if (item)
        {
            return item;
        }

        if (_isExpandable)
        {
            return AddItem(true);
        }
        
        throw new Exception("Pool expend");
    }

    public T GetRandomItem()
    {
        if (!_isInitialized)
        {
            InitializePool();
            _isInitialized = true;
        }

        int startIndex = Random.Range(0, _pool.Count);
        
        T item = FindFreeElement(startIndex, _pool.Count);
        if (item)
        {
            return item;
        }

        item = FindFreeElement(0, startIndex);
        if (item)
        {
            return item;
        }
        
        if (_isExpandable)
        {
            return AddItem(true);
        }
        
        throw new Exception("Pool expend");
    }

    private T FindFreeElement(int startIndex, int endIndex)
    {
        for (int i = startIndex; i < endIndex; i++)
        {
            if (!_pool[i].gameObject.activeSelf)
            {
                _pool[i].gameObject.SetActive(true);
                return _pool[i];
            }
        }

        return null;
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