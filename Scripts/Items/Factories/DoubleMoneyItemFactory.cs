using UnityEngine;

public sealed class DoubleMoneyItemFactory<T> : ItemFactory<T> where T: Item
{
    private readonly RunProgress _runProgress;
    private readonly ActiveItemsUI _activeItemsUI;
    private readonly Run _run;
    private readonly ItemsActiveStates _itemsActiveStates;
    
    public DoubleMoneyItemFactory(
        RunProgress runProgress,
        ActiveItemsUI activeItemsUI,
        Run run,
        ItemsActiveStates itemsActiveStates)
    {
        _runProgress = runProgress;
        _activeItemsUI = activeItemsUI;
        _run = run;
        _itemsActiveStates = itemsActiveStates;
    }
    
    protected override string PrefabName => "DoubleMoney";

    public override T CreateItem()
    {
        GameObject doubleMoneyObject = Object.Instantiate(_prefab);
        var doubleMoney = doubleMoneyObject.AddComponent<DoubleMoney>();
        doubleMoney.Init(_runProgress, _activeItemsUI, _run, _itemsActiveStates);

        return doubleMoney as T;
    }
}