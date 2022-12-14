public sealed class DoubleScore : ActivatableItem<DoubleMoney>
{
    private RunProgress _runProgress;

    protected override float ActiveTime => 10;
    
    protected override ItemType ActiveItemType => ItemType.DoubleScore;
    
    public void Init(RunProgress runProgress, ActiveItemsUI activeItemsUI)
    {
        base.Init(activeItemsUI);

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