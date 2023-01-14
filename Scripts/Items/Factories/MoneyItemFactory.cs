using Unity.VisualScripting;
using UnityEngine;

public sealed class MoneyItemFactory<T> : ItemFactory<T> where T: Item
{
    private readonly RunProgress _runProgress;
    
    private readonly bool _isAutoHiding;
    private readonly bool _isAutoActivating;

    public MoneyItemFactory(RunProgress runProgress, bool isAutoActivating, bool isAutoHiding)
    {
        _runProgress = runProgress;
        _isAutoActivating = isAutoActivating;
        _isAutoHiding = isAutoHiding;
    }

    protected override string PrefabName => "Money";

    public override T CreateItem()
    {
        Money money = CreateMoney();
        money.Init(_runProgress, false);

        return money as T;
    }

    private Money CreateMoney()
    {
        GameObject moneyObject = Object.Instantiate(_prefab);
        return moneyObject.AddComponent<Money>();
    }
}
