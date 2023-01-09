using UnityEngine;

public abstract class ItemFactory<T> : AbstractFactory<T> where T : MonoBehaviour
{
    protected abstract string PrefabName { get; }

    protected readonly GameObject _prefab;
    
    protected ItemFactory()
    {
        _prefab = Resources.Load(PrefabName) as GameObject;
    }
}
