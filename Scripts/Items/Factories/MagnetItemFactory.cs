using UnityEngine;

public sealed class MagnetItemFactory<T> : ItemFactory<T> where T : Item
{
    protected override string PrefabName => "Items/Magnet";

    private readonly Run _run;
    private readonly ActiveItemsUI _activeItemsUI;
    
    public MagnetItemFactory(ActiveItemsUI activeItemsUI, Run run)
    {
        _activeItemsUI = activeItemsUI;
        _run = run;
    }

    public override T CreateItem()
    {
        GameObject highJumpObject = Object.Instantiate(_prefab);
        Magnet highJump = highJumpObject.AddComponent<Magnet>();
        highJump.Init(_activeItemsUI, _run);

        return highJump as T;
    }
}