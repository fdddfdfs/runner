using StarterAssets;

public sealed class Spring : Item
{
    private const int PickupItemEffectTimeMilliseconds = 5000;
    
    private Effects _effects;
    
    public static int Weight => 1;
    
    public void Init(Run run, Effects effects)
    {
        base.Init(run);
        _effects = effects;
    }
    
    public override void PickupItem(ThirdPersonController player)
    {
        base.PickupItem(player);
        
        player.ChangeGravitable(player.Gravitables[typeof(SpringGravity)]);
        _effects.ActivateEffect(EffectType.PickupItem, transform.position, PickupItemEffectTimeMilliseconds);
    }
}