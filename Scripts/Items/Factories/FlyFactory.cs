using UnityEngine;

public class FlyFactory<T> : Factory<T> where T:Item
{
    private ActiveItemsUI _activeItemsUI;
    
    protected override string PrefabName => "Fly";

    public FlyFactory(ActiveItemsUI activeItemsUI)
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