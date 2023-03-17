using UnityEngine;

public sealed class FlyItemFactory<T> : ItemFactory<T> where T:Item
{
    private readonly ActiveItemsUI _activeItemsUI;
    private readonly Run _run;
    private readonly ItemsActiveStates _itemsActiveStates;
    private readonly Effects _effects;
    
    protected override string PrefabName => "Items/Fly";

    public FlyItemFactory(
        ActiveItemsUI activeItemsUI,
        Run run,
        ItemsActiveStates itemsActiveStates,
        Effects effects)
    {
        _activeItemsUI = activeItemsUI;
        _run = run;
        _itemsActiveStates = itemsActiveStates;
        _effects = effects;
    }

    public override T CreateItem()
    {
        GameObject flyObject = Object.Instantiate(_prefab);
        var fly = flyObject.AddComponent<Fly>();
        fly.Init(_activeItemsUI, _run, _itemsActiveStates, _effects);

        return fly as T;
    }
}