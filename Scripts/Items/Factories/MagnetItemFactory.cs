using UnityEngine;

public class MagnetItemFactory<T> : ItemFactory<T> where T : Item
{
    protected override string PrefabName => "Magnet";

    private ActiveItemsUI _activeItemsUI;
    
    public MagnetItemFactory(ActiveItemsUI activeItemsUI)
    {
        _activeItemsUI = activeItemsUI;
    }

    public override T CreateItem()
    {
        GameObject highJumpObject = Object.Instantiate(_prefab);
        Magnet highJump = highJumpObject.AddComponent<Magnet>();
        highJump.Init(_activeItemsUI);

        return highJump as T;
    }
}