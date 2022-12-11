using System;
using System.Collections;
using StarterAssets;
using UnityEngine;

public class HighJump : ActivatableItem<HighJump>
{
    private const float Multiplayer = 3;
    
    private static float _baseHeight = -1;

    private ThirdPersonController _player;

    public void Init(float baseJumpHeight, ActiveItemsUI activeItemUI)
    {
        base.Init(activeItemUI);

        _baseHeight = baseJumpHeight;
    }

    public override void PickupItem(ThirdPersonController player)
    {
        base.PickupItem(player);

        _player = player;
    }

    protected override float ActiveTime => 10;
    protected override ItemType ActiveItemType => ItemType.HighJump;
    protected override void Activate()
    {
        _player.JumpHeight = _baseHeight * Multiplayer;
    }

    protected override void Deactivate()
    {
        _player.JumpHeight = _baseHeight;
    }
}