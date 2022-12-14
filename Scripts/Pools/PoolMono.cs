using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class PoolMono<T> where T: MonoBehaviour
{
    private const int DefaultStartCount = 20;

    private readonly List<T> _pool;
    
    private readonly bool _isExpandable;

    private Transform _parent;
    private GameObject _prefab;
    private int _poolSize;

    public PoolMono(
        GameObject prefab,
        Transform parent = null,
        bool isExpandable = false,
        int startPoolSize = DefaultStartCount)
    {
        _pool = new List<T>();

        _prefab = prefab;
        _parent = parent;
        _poolSize = startPoolSize;
        _isExpandable = isExpandable;
        
        InitializePool();
    }

    public PoolMono(List<T> spawnedPrefabs)
    {
        for (int i = 0; i < spawnedPrefabs.Count; i++)
        {
            spawnedPrefabs[i].gameObject.SetActive(false);
        }

        _pool = spawnedPrefabs;

        _poolSize = spawnedPrefabs.Count;
        _isExpandable = false;
    }

    public T GetItem()
    {
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
        else
        {
            throw new Exception("Pool expend");
        }
    }

    private void InitializePool()
    {
        for (int i = _pool.Count; i < _poolSize; i++)
        {
            AddItem();
        }
    }

    private T AddItem(bool isActive = false)
    {
        GameObject newItem = Object.Instantiate(_prefab, _parent);
        newItem.SetActive(isActive);
        T component = newItem.GetComponent<T>();
        _pool.Add(component);

        return component;
    }
}