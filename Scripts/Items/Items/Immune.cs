using StarterAssets;

public sealed class Immune : ActivatableItem
{
    private static ThirdPersonController _player;
    
    public static int Weight => 1;
    
    protected override float ActiveTime => 10;
    
    protected override ItemType ActiveItemType => ItemType.Immune;
    
    public override void PickupItem(ThirdPersonController player)
    {
        _player = player;
        
        base.PickupItem(player);
    }
    
    protected override void Activate()
    {
        _player.ChangeHittable(_player.Hittables[typeof(ImmuneHittable)]);
        _player.PlayerAnimator.ChangeAnimator(typeof(PlayerImmuneAnimator));
    }

    protected override void Deactivate()
    {
        _player.ChangeHittable(_player.Hittables[typeof(PlayerHittable)]);
        _player.PlayerAnimator.ChangeAnimator(typeof(PlayerDefaultAnimator));
    }
}