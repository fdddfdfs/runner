﻿public sealed class DoubleScore : ActivatableItem
{
    private const float AddedSecondsPerLevel = 2;
    private const float BaseDuration = 10;
    
    private RunProgress _runProgress;
    
    public static int Weight => 1;

    protected override float ActiveTime => BaseDuration + AddedSecondsPerLevel * Stats.DoubleMoneyLevel.Value;
    
    protected override ItemType ActiveItemType => ItemType.DoubleScore;
    
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
    
    protected override void Activate()
    {
        _runProgress.ChangeScoreMultiplier(RunProgress.DefaultScoreMultiplier * 2);
    }

    protected override void Deactivate()
    {
        _runProgress.ChangeScoreMultiplier();
    }
}