using System;
using UnityEngine;

public abstract class ItemFactory<T> : AbstractFactory<T> where T : MonoBehaviour
{
    protected abstract string PrefabName { get; }

    protected readonly GameObject _prefab;
    
    protected ItemFactory()
    {
        if (PrefabName == null) return; 
        
        _prefab = Resources.Load(PrefabName) as GameObject;

        if (!_prefab)
        {
            throw new Exception($"Cannot find resource prefab named {PrefabName}");
        }
    }
}
