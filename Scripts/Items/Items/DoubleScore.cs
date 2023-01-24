public sealed class DoubleScore : ActivatableItem
{
    private RunProgress _runProgress;
    
    public static int Weight => 1;

    protected override float ActiveTime => 10;
    
    protected override ItemType ActiveItemType => ItemType.DoubleScore;
    
    public void Init(RunProgress runProgress, ActiveItemsUI activeItemsUI, Run run, ItemsActiveStates itemsActiveStates)
    {
        base.Init(activeItemsUI, run, itemsActiveStates);

        _runProgress = runProgress;
    }
    
    protected override void Activate()
    {
        _runProgress.ChangeScoreMultiplier(RunProgress.DefaultScoreMultiplier * 2);
    }

    protected override void Deactivate()
    {
        _runProgress.ChangeScoreMultiplier();
    }
}