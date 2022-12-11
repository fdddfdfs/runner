using UnityEngine;

public sealed class MoneyFactory<T> : Factory<T> where T: Item
{
    private readonly RunProgress _runProgress;

    public MoneyFactory(RunProgress runProgress)
    {
        _runProgress = runProgress;
    }

    protected override string PrefabName => "Money";

    public override T CreateItem()
    {
        GameObject moneyObject = Object.Instantiate(_prefab);
        Money money = moneyObject.AddComponent<Money>();
        money.Init(_runProgress);

        return money as T;
    }
}
