using UnityEngine;

public sealed class SpringItemFactory<T> : ItemFactory<T> where T : Item
{
    private readonly Run _run;
    private readonly Effects _effects;
    
    protected override string PrefabName => "Items/Spring";

    public SpringItemFactory(Run run, Effects effects)
    {
        _run = run;
        _effects = effects;
    }

    public override T CreateItem()
    {
        GameObject springObject = Object.Instantiate(_prefab);
        Spring spring = springObject.AddComponent<Spring>();
        spring.Init(_run, _effects);

        return spring as T;
    }
}