using UnityEngine;

public class ImmuneFactory<T> : Factory<T> where T: Item
{
    private readonly ActiveItemsUI _activeItemsUI;

    public ImmuneFactory(ActiveItemsUI activeItemsUI)
    {
        _activeItemsUI = activeItemsUI;
    }
    
    protected override string PrefabName => "Immune";

    public override T CreateItem()
    {
        GameObject immuneObject = Object.Instantiate(_prefab);
        Immune immune = immuneObject.AddComponent<Immune>();
        immune.Init(_activeItemsUI);

        return immune as T;
    }
}