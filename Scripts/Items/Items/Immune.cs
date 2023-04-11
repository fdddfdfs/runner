﻿using StarterAssets;

public sealed class Immune : ActivatableItem
{
    private const float AddedSecondsPerLevel = 2;
    private const float BaseDuration = 10;
    
    private static ThirdPersonController _player;
    
    public static int Weight => 1;

    protected override float ActiveTime => BaseDuration + AddedSecondsPerLevel * Stats.ImmuneLevel.Value;
    
    protected override ItemType ActiveItemType => ItemType.Immune;

    public override void PickupItem(ThirdPersonController player)
    {
        _player = player;
        
        Sounds.Instance.PlaySound(2, "Immune");
        Achievements.Instance.GetAchievement("Item_5");
        
        base.PickupItem(player);
    }
    
    protected override void Activate()
    {
        _player.PlayerStateMachine.ChangeState(typeof(ImmuneState));
    }

    protected override void Deactivate()
    {
        _player.PlayerStateMachine.ChangeStateSafely(typeof(ImmuneState), typeof(RunState));
    }
}