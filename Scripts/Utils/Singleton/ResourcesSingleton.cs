using UnityEngine;

public abstract class ResourcesSingleton<T, TResourceName> : MonoBehaviour 
    where T: MonoBehaviour
    where TResourceName : ResourceName, new()
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (!_instance)
            {
                Initialize();
            }

            return _instance;
        }
    }

    private static void Initialize()
    {
        ResourceName  resourceName = new TResourceName();
        _instance = ResourcesLoader.InstantiateLoadComponent<T>(resourceName.Name);
        DontDestroyOnLoad(_instance.gameObject);
    }
}