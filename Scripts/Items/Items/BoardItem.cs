using StarterAssets;

public sealed class BoardItem : Item
{
    private Effects _effects;
    
    public static int Weight => 1;
    
    public void Init(ICancellationTokenProvider cancellationTokenProvider, Effects effects)
    {
        _effects = effects;
        
        base.Init(cancellationTokenProvider);
    }
    
    public override void PickupItem(ThirdPersonController player)
    {
        base.PickupItem(player);

        Stats.BoardCount.Value += 1;
        
        Sounds.Instance.PlayRandomSounds(2, "Item");
        _effects.ActivateEffect(EffectType.PickupItem, transform.position);
    }
}