using UnityEngine;

public sealed class MagnetItemFactory<T> : ItemFactory<T> where T : Item
{
    protected override string PrefabName => "Items/Magnet";

    private readonly Run _run;
    private readonly ActiveItemsUI _activeItemsUI;
    private readonly ItemsActiveStates _itemsActiveStates;
    
    public MagnetItemFactory(ActiveItemsUI activeItemsUI, Run run, ItemsActiveStates itemsActiveStates)
    {
        _activeItemsUI = activeItemsUI;
        _run = run;
        _itemsActiveStates = itemsActiveStates;
    }

    public override T CreateItem()
    {
        GameObject highJumpObject = Object.Instantiate(_prefab);
        Magnet highJump = highJumpObject.AddComponent<Magnet>();
        highJump.Init(_activeItemsUI, _run, _itemsActiveStates);

        return highJump as T;
    }
}