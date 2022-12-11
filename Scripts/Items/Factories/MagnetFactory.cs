using UnityEngine;

public class MagnetFactory<T> : Factory<T> where T : Item
{
    protected override string PrefabName => "Magnet";

    private ActiveItemsUI _activeItemsUI;
    
    public MagnetFactory(ActiveItemsUI activeItemsUI)
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