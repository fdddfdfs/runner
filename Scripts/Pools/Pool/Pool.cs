using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class Pool
{
    private const int StartCount = 20;

    protected readonly List<GameObject> _pool;
    
    private readonly bool _isExpandable;

    protected bool _isInitialized;

    protected Pool(bool isExpandable = false)
    {
        _pool = new List<GameObject>();
        _isExpandable = isExpandable;
    }

    public GameObject GetItem()
    {
        if (!_isInitialized)
        {
            InitializePool();
            _isInitialized = true;
        }
        
        for (int i = 0; i < _pool.Count; i++)
        {
            if (!_pool[i].activeSelf)
            {
                _pool[i].SetActive(true);
                return _pool[i];
            }
        }

        if (_isExpandable)
        {
            return AddItem(true);
        }
        else
        {
            throw new Exception("Pool expend");
        }
    }

    private void InitializePool()
    {
        for (int i = _pool.Count; i < StartCount; i++)
        {
            AddItem();
        }
    }

    protected abstract GameObject AddItem(bool isActive = false);
}
