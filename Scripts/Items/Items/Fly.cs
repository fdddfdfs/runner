using System;
using StarterAssets;
using UnityEngine;

public sealed class Fly : ActivatableItem
{
    private static ThirdPersonController _player;
    
    public static int Weight => 5;
    
    protected override float ActiveTime => 10;

    protected override ItemType ActiveItemType => ItemType.Fly;

    public override void PickupItem(ThirdPersonController player)
    {
        _player = player;
        
        base.PickupItem(player);
    }

    protected override void Activate()
    {
        if (_player.Gravitables[typeof(FlyGravity)] is not FlyGravity flyGravity)
        {
            throw new Exception("Unable get FlyGravity from players gravities");
        }

        flyGravity.SetGravityLength(ActiveTime);
        _player.PlayerStateMachine.ChangeState(typeof(FlyState));
    }

    protected override void Deactivate()
    {
    }
}