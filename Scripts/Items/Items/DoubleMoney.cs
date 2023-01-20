public sealed class DoubleMoney : ActivatableItem
{
    private RunProgress _runProgress;
    private ActiveItemsUI _activeItemsUI;

    public static int Weight => 1;
    
    protected override float ActiveTime => 10;
    
    protected override ItemType ActiveItemType => ItemType.DoubleMoney;
    
    public void Init(RunProgress runProgress, ActiveItemsUI activeItemsUI, Run run)
    {
        base.Init(activeItemsUI, run);
        
        _runProgress = runProgress;
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