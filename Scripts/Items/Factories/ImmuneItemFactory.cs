using UnityEngine;

public sealed class ImmuneItemFactory<T> : ItemFactory<T> where T: Item
{
    private readonly ActiveItemsUI _activeItemsUI;
    private readonly Run _run;
    private readonly ItemsActiveStates _itemsActiveStates;

    public ImmuneItemFactory(ActiveItemsUI activeItemsUI, Run run, ItemsActiveStates itemsActiveStates)
    {
        _activeItemsUI = activeItemsUI;
        _run = run;
        _itemsActiveStates = itemsActiveStates;
    }
    
    protected override string PrefabName => "Immune";

    public override T CreateItem()
    {
        GameObject immuneObject = Object.Instantiate(_prefab);
        var immune = immuneObject.AddComponent<Immune>();
        immune.Init(_activeItemsUI, _run, _itemsActiveStates);

        return immune as T;
    }
}