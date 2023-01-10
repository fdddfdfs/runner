using System;
using StarterAssets;

public sealed class Fly : ActivatableItem<Fly>
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
        if (_player.Gravitables[typeof(FlyGravity)] is not FlyGravity flyGravity)
        {
            throw new Exception("Unable get FlyGravity from players gravities");
        }

        flyGravity.SetGravityLength(ActiveTime);
        _player.ChangeGravitable(flyGravity);
    }

    protected override void Deactivate()
    {
        _player.ChangeGravitable(_player.Gravitables[typeof(DefaultGravity)]);
    }
}