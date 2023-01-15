using UnityEngine;

public sealed class SpringItemFactory<T> : ItemFactory<T> where T : Item
{
    private readonly Run _run;
    
    protected override string PrefabName => "Spring";

    public SpringItemFactory(Run run)
    {
        _run = run;
    }

    public override T CreateItem()
    {
        GameObject springObject = Object.Instantiate(_prefab);
        Spring spring = springObject.AddComponent<Spring>();
        spring.Init(_run);

        return spring as T;
    }
}