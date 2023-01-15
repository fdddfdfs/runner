using UnityEngine;

public sealed class ImmuneItemFactory<T> : ItemFactory<T> where T: Item
{
    private readonly ActiveItemsUI _activeItemsUI;
    private readonly Run _run;

    public ImmuneItemFactory(ActiveItemsUI activeItemsUI, Run run)
    {
        _activeItemsUI = activeItemsUI;
        _run = run;
    }
    
    protected override string PrefabName => "Immune";

    public override T CreateItem()
    {
        GameObject immuneObject = Object.Instantiate(_prefab);
        Immune immune = immuneObject.AddComponent<Immune>();
        immune.Init(_activeItemsUI, _run);

        return immune as T;
    }
}