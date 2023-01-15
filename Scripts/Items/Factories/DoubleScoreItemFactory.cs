using UnityEngine;

public sealed class DoubleScoreItemFactory<T> : ItemFactory<T> where T: Item
{
    private readonly RunProgress _runProgress;
    private readonly ActiveItemsUI _activeItemsUI;
    private readonly Run _run;
    
    public DoubleScoreItemFactory(RunProgress runProgress, ActiveItemsUI activeItemsUI, Run run)
    {
        _runProgress = runProgress;
        _activeItemsUI = activeItemsUI;
        _run = run;
    }
    
    protected override string PrefabName => "DoubleScore";
    
    public override T CreateItem()
    {
        GameObject doubleScoreObject = Object.Instantiate(_prefab);
        DoubleScore doubleScore = doubleScoreObject.AddComponent<DoubleScore>();
        doubleScore.Init(_runProgress, _activeItemsUI, _run);

        return doubleScore as T;
    }
}