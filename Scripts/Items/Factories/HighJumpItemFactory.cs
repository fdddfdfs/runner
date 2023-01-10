using StarterAssets;
using UnityEngine;

public sealed class HighJumpItemFactory<T> : ItemFactory<T> where T : Item
{
    protected override string PrefabName => "HighJump";

    private readonly float _baseJumpHeight;
    private readonly ActiveItemsUI _activeItemsUI;

    public HighJumpItemFactory(ThirdPersonController player, ActiveItemsUI activeItemsUI)
    {
        _baseJumpHeight = player.JumpHeight;
        _activeItemsUI = activeItemsUI;
    }

    public override T CreateItem()
    {
        GameObject highJumpObject = Object.Instantiate(_prefab);
        HighJump highJump = highJumpObject.AddComponent<HighJump>();
        highJump.Init(_baseJumpHeight, _activeItemsUI);

        return highJump as T;
    }
}