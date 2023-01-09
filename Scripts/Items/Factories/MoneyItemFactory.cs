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
        GameObject moneyObject = Object.Instantiate(_prefab);
        Money money = moneyObject.AddComponent<Money>();
        money.Init(_runProgress, _isAutoActivating, _isAutoHiding);

        return money as T;
    }
}
