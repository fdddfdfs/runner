using System.Collections.Generic;
using UnityEngine;

public class GameObjectPoolMono<T> : PoolMono<T> where T: MonoBehaviour
{
    private readonly Transform _parent;
    private readonly GameObject _prefab;
    
    public GameObjectPoolMono(
        GameObject prefab,
        Transform parent = null,
        bool isExpandable = false,
        int startPoolSize = DefaultStartCount) 
        : base(isExpandable, startPoolSize)
    {
        _prefab = prefab;
        _parent = parent;
    }

    public GameObjectPoolMono(List<T> spawnedPrefabs)
    {
        for (int i = 0; i < spawnedPrefabs.Count; i++)
        {
            spawnedPrefabs[i].gameObject.SetActive(false);
        }

        _pool = spawnedPrefabs;
        _isInitialized = true;
    }
    
    protected override T AddItem(bool isActive = false)
    {
        GameObject newItem = Object.Instantiate(_prefab, _parent);
        newItem.SetActive(isActive);
        T component = newItem.GetComponent<T>();
        _pool.Add(component);

        return component;
    }
}