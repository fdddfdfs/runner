using UnityEngine;

public abstract class Singletone<T> : MonoBehaviour where T: MonoBehaviour
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
        GameObject gameObject = new();
        _instance =  gameObject.AddComponent<T>();
        DontDestroyOnLoad(gameObject);
    }
}