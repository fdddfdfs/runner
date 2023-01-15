using UnityEngine;

public sealed class DoubleMoneyItemFactory<T> : ItemFactory<T> where T: Item
{
    private readonly RunProgress _runProgress;
    private readonly ActiveItemsUI _activeItemsUI;
    private readonly Run _run;
    
    public DoubleMoneyItemFactory(RunProgress runProgress, ActiveItemsUI activeItemsUI, Run run)
    {
        _runProgress = runProgress;
        _activeItemsUI = activeItemsUI;
        _run = run;
    }
    
    protected override string PrefabName => "DoubleMoney";

    public override T CreateItem()
    {
        GameObject doubleMoneyObject = Object.Instantiate(_prefab);
        DoubleMoney doubleMoney = doubleMoneyObject.AddComponent<DoubleMoney>();
        doubleMoney.Init(_runProgress, _activeItemsUI, _run);

        return doubleMoney as T;
    }
}