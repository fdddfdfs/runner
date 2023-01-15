using Unity.VisualScripting;
using UnityEngine;

public sealed class MoneyItemFactory<T> : ItemFactory<T> where T: Item
{
    private readonly RunProgress _runProgress;
    
    private readonly bool _isAutoHiding;
    private readonly bool _isAutoActivating;
    private readonly Run _run;

    public MoneyItemFactory(RunProgress runProgress, Run run, bool isAutoActivating, bool isAutoHiding)
    {
        _runProgress = runProgress;
        _isAutoActivating = isAutoActivating;
        _isAutoHiding = isAutoHiding;
        _run = run;
    }

    protected override string PrefabName => "Money";

    public override T CreateItem()
    {
        Money money = CreateMoney();
        money.Init(_runProgress, _run, false);

        return money as T;
    }

    private Money CreateMoney()
    {
        GameObject moneyObject = Object.Instantiate(_prefab);
        return moneyObject.AddComponent<Money>();
    }
}
