using UnityEngine;

public sealed class DoubleMoneyItemFactory<T> : ItemFactory<T> where T: Item
{
    private readonly RunProgress _runProgress;
    private readonly ActiveItemsUI _activeItemsUI;
    
    public DoubleMoneyItemFactory(RunProgress runProgress, ActiveItemsUI activeItemsUI)
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