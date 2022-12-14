using UnityEngine;

public sealed class DoubleScoreFactory<T> : Factory<T> where T: Item
{
    private readonly RunProgress _runProgress;
    private readonly ActiveItemsUI _activeItemsUI;
    
    public DoubleScoreFactory(RunProgress runProgress, ActiveItemsUI activeItemsUI)
    {
        _runProgress = runProgress;
        _activeItemsUI = activeItemsUI;
    }
    
    protected override string PrefabName => "DoubleScore";
    
    public override T CreateItem()
    {
        GameObject doubleScoreObject = Object.Instantiate(_prefab);
        DoubleScore doubleScore = doubleScoreObject.AddComponent<DoubleScore>();
        doubleScore.Init(_runProgress, _activeItemsUI);

        return doubleScore as T;
    }
}