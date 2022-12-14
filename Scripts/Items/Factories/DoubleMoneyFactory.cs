using UnityEngine;

public sealed class DoubleMoneyFactory<T> : Factory<T> where T: Item
{
    private readonly RunProgress _runProgress;
    private readonly ActiveItemsUI _activeItemsUI;
    
    public DoubleMoneyFactory(RunProgress runProgress, ActiveItemsUI activeItemsUI)
    {
        _runProgress = runProgress;
        _activeItemsUI = activeItemsUI;
    }
    
    protected override string PrefabName => "DoubleMoney";

    public override T CreateItem()
    {
        GameObject doubleMoneyObject = Object.Instantiate(_prefab);
        DoubleMoney doubleMoney = doubleMoneyObject.AddComponent<DoubleMoney>();
        doubleMoney.Init(_runProgress, _activeItemsUI);

        return doubleMoney as T;
    }
}