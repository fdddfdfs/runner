using UnityEngine;

public sealed class FlyItemFactory<T> : ItemFactory<T> where T:Item
{
    private readonly ActiveItemsUI _activeItemsUI;
    private readonly Run _run;
    
    protected override string PrefabName => "Fly";

    public FlyItemFactory(ActiveItemsUI activeItemsUI, Run run)
    {
        _activeItemsUI = activeItemsUI;
        _run = run;
    }

    public override T CreateItem()
    {
        GameObject flyObject = Object.Instantiate(_prefab);
        Fly fly = flyObject.AddComponent<Fly>();
        fly.Init(_activeItemsUI, _run);

        return fly as T;
    }
}