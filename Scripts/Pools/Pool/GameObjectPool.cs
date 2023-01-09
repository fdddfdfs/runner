using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameObjectPool : Pool
{
    private readonly GameObject _prefab;
    private readonly Transform _parent;
    
    public GameObjectPool(
        GameObject prefab,
        Transform parent = null,
        bool isExpandable = false,
        bool isPrefabPartOfPool = false) : 
        base(isExpandable)
    {
        _prefab = prefab;
        _parent = parent;

        if (isPrefabPartOfPool)
        {
            _pool.Add(prefab);
            prefab.SetActive(false);
        }
    }

    public GameObjectPool(List<GameObject> instantiatedPrefabs)
    {
        for (int i = 0; i < instantiatedPrefabs.Count; i++)
        {
            _pool.Add(instantiatedPrefabs[i]);
            instantiatedPrefabs[i].SetActive(false);
        }

        _isInitialized = true;
    }
    
    protected override GameObject AddItem(bool isActive = false) 
    {
        GameObject newItem = Object.Instantiate(_prefab, _parent);

        newItem.SetActive(isActive);
        _pool.Add(newItem);

        return newItem;
    }
}
