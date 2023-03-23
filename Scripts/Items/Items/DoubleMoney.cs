using StarterAssets;

public sealed class DoubleMoney : ActivatableItem
{
    private const float AddedSecondsPerLevel = 2;
    private const float BaseDuration = 10;
    
    private RunProgress _runProgress;
    private ActiveItemsUI _activeItemsUI;

    public static int Weight => 1;
    
    protected override float ActiveTime => BaseDuration + AddedSecondsPerLevel * Stats.DoubleMoneyLevel.Value;
    
    protected override ItemType ActiveItemType => ItemType.DoubleMoney;
    
    public void Init(
        RunProgress runProgress,
        ActiveItemsUI activeItemsUI,
        Run run,
        ItemsActiveStates itemsActiveStates,
        Effects effects)
    {
        base.Init(activeItemsUI, run, itemsActiveStates, effects);
        
        _runProgress = runProgress;
    }
    
    public override void PickupItem(ThirdPersonController player)
    {
        Sounds.Instance.PlayRandomSounds(2, "Item");
        
        base.PickupItem(player);
    }

    protected override void Activate()
    {
        _runProgress.ChangeMoneyMultiplier(RunProgress.DefaultMoneyMultiplier * 2);
    }

    protected override void Deactivate()
    {
        _runProgress.ChangeMoneyMultiplier();
    }
}