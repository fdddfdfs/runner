using StarterAssets;
using UnityEngine;

public class HighJumpFactory<T> : Factory<T> where T : Item
{
    protected override string PrefabName => "HighJump";

    private float _baseJumpHeight;
    private ActiveItemsUI _activeItemsUI;

    public HighJumpFactory(ThirdPersonController player, ActiveItemsUI activeItemsUI)
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