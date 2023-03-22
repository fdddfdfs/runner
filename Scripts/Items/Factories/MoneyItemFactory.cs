using UnityEngine;

public sealed class MoneyItemFactory<T> : ItemFactory<T> where T: Item
{
    private readonly RunProgress _runProgress;
    
    private readonly bool _isAutoHiding;
    private readonly bool _isAutoActivating;
    private readonly Run _run;
    private readonly Effects _effects;

    protected override string PrefabName => "Items/Money";
    
    public MoneyItemFactory(RunProgress runProgress, Run run, bool isAutoActivating, bool isAutoHiding, Effects effects)
    {
        _runProgress = runProgress;
        _isAutoActivating = isAutoActivating;
        _isAutoHiding = isAutoHiding;
        _run = run;
        _effects = effects;
    }

    public override T CreateItem()
    {
        Money money = CreateMoney();
        money.Init(_runProgress, _run, false, _effects);

        return money as T;
    }

    private Money CreateMoney()
    {
        GameObject moneyObject = Object.Instantiate(_prefab);
        return moneyObject.AddComponent<Money>();
    }
}
