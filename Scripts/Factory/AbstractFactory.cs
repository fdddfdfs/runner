using UnityEngine;

public abstract class AbstractFactory<T> where T : MonoBehaviour
{
    public abstract T CreateItem();
}