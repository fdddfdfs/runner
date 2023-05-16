using StarterAssets;

public sealed class BoardItem : Item
{
    private Effects _effects;
    private PickupCar _pickupCar;
    
    public static int Weight => 1;
    
    public void Init(ICancellationTokenProvider cancellationTokenProvider, Effects effects, PickupCar pickupCar)
    {
        _effects = effects;
        _pickupCar = pickupCar;
        
        base.Init(cancellationTokenProvider);
    }
    
    public override void PickupItem(ThirdPersonController player)
    {
        base.PickupItem(player);

        Stats.BoardCount.Value += 1;
        
        Sounds.Instance.PlayRandomSounds(2, "Item");
        _effects.ActivateEffect(EffectType.PickupItem, transform.position);
        _pickupCar.ShowPickupCar();
    }
}