using System;
using StarterAssets;

public sealed class Fly : ActivatableItem
{
    private const float AddedSecondsPerLevel = 2;
    private const float BaseDuration = 10;
    
    private static ThirdPersonController _player;
    
    public static int Weight => 1;
    
    protected override float ActiveTime => BaseDuration + AddedSecondsPerLevel * Stats.DoubleMoneyLevel.Value;

    protected override ItemType ActiveItemType => ItemType.Fly;

    public override void PickupItem(ThirdPersonController player)
    {
        _player = player;
        
        Sounds.Instance.PlaySound(2, "Fly");
        Achievements.Instance.GetAchievement("Item_3");
        
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