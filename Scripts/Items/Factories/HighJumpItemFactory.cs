using StarterAssets;
using UnityEngine;

public sealed class HighJumpItemFactory<T> : ItemFactory<T> where T : Item
{
    protected override string PrefabName => "HighJump";

    private readonly float _baseJumpHeight;
    private readonly ActiveItemsUI _activeItemsUI;
    private readonly Run _run;

    public HighJumpItemFactory(ThirdPersonController player, ActiveItemsUI activeItemsUI, Run run)
    {
        _baseJumpHeight = player.JumpHeight;
        _activeItemsUI = activeItemsUI;
        _run = run;
    }

    public override T CreateItem()
    {
        GameObject highJumpObject = Object.Instantiate(_prefab);
        HighJump highJump = highJumpObject.AddComponent<HighJump>();
        highJump.Init(_baseJumpHeight, _activeItemsUI, _run);

        return highJump as T;
    }
}