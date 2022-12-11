using UnityEngine;

public abstract class Factory<T> where T : MonoBehaviour
{
    protected abstract string PrefabName { get; }

    protected readonly GameObject _prefab;
    
    protected Factory()
    {
        _prefab = Resources.Load(PrefabName) as GameObject;
    }
    
    public abstract T CreateItem();
}
