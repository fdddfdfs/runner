using System.Collections;
using StarterAssets;
using UnityEngine;

public class Fly : ActivatableItem<Fly>
{
    private static ThirdPersonController _player;
    
    protected override float ActiveTime => 10;

    protected override ItemType ActiveItemType => ItemType.Fly;

    public override void PickupItem(ThirdPersonController player)
    {
        _player = player;
        
        base.PickupItem(player);
    }

    protected override void Activate()
    {
        _player.ChangeGravitable(_player.Gravitables[typeof(FlyGravity)]);
    }

    protected override void Deactivate()
    {
        _player.ChangeGravitable(_player.Gravitables[typeof(DefaultGravity)]);
    }
}