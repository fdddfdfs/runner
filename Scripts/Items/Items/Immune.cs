using StarterAssets;

public sealed class Immune : ActivatableItem
{
    private static ThirdPersonController _player;
    
    public static int Weight => 10;
    
    protected override float ActiveTime => 10;
    
    protected override ItemType ActiveItemType => ItemType.Immune;
    
    public override void PickupItem(ThirdPersonController player)
    {
        _player = player;
        
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