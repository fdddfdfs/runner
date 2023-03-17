using UnityEngine;

public sealed class DoubleScoreItemFactory<T> : ItemFactory<T> where T: Item
{
    private readonly RunProgress _runProgress;
    private readonly ActiveItemsUI _activeItemsUI;
    private readonly Run _run;
    private readonly ItemsActiveStates _itemsActiveStates;
    private readonly Effects _effects;
    
    protected override string PrefabName => "Items/DoubleScore";
    
    public DoubleScoreItemFactory(
        RunProgress runProgress,
        ActiveItemsUI activeItemsUI,
        Run run,
        ItemsActiveStates itemsActiveStates,
        Effects effects)
    {
        _runProgress = runProgress;
        _activeItemsUI = activeItemsUI;
        _run = run;
        _itemsActiveStates = itemsActiveStates;
        _effects = effects;
    }

    public override T CreateItem()
    {
        GameObject doubleScoreObject = Object.Instantiate(_prefab);
        var doubleScore = doubleScoreObject.AddComponent<DoubleScore>();
        doubleScore.Init(_runProgress, _activeItemsUI, _run, _itemsActiveStates, _effects);

        return doubleScore as T;
    }
}