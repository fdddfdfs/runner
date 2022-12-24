using UnityEngine;

public class SpringFactory<T> : Factory<T> where T : Item
{
    protected override string PrefabName => "Spring";
    
    public override T CreateItem()
    {
        GameObject springObject = Object.Instantiate(_prefab);
        Spring spring = springObject.AddComponent<Spring>();
        spring.Init();

        return spring as T;
    }
}