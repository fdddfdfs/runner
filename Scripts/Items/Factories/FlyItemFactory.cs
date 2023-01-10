using UnityEngine;

public sealed class FlyItemFactory<T> : ItemFactory<T> where T:Item
{
    private readonly ActiveItemsUI _activeItemsUI;
    
    protected override string PrefabName => "Fly";

    public FlyItemFactory(ActiveItemsUI activeItemsUI)
    {
        _activeItemsUI = activeItemsUI;
    }

    public override T CreateItem()
    {
        GameObject flyObject = Object.Instantiate(_prefab);
        Fly fly = flyObject.AddComponent<Fly>();
        fly.Init(_activeItemsUI);

        return fly as T;
    }
}