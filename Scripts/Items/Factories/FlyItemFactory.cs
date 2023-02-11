using UnityEngine;

public sealed class FlyItemFactory<T> : ItemFactory<T> where T:Item
{
    private readonly ActiveItemsUI _activeItemsUI;
    private readonly Run _run;
    private readonly ItemsActiveStates _itemsActiveStates;
    
    protected override string PrefabName => "Items/Fly";

    public FlyItemFactory(ActiveItemsUI activeItemsUI, Run run, ItemsActiveStates itemsActiveStates)
    {
        _activeItemsUI = activeItemsUI;
        _run = run;
        _itemsActiveStates = itemsActiveStates;
    }

    public override T CreateItem()
    {
        GameObject flyObject = Object.Instantiate(_prefab);
        var fly = flyObject.AddComponent<Fly>();
        fly.Init(_activeItemsUI, _run, _itemsActiveStates);

        return fly as T;
    }
}