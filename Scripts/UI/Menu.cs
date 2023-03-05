using UnityEngine;

public abstract class Menu : MonoBehaviour
{
    [SerializeField] protected GameObject _menu;

    public virtual void ChangeMenuActive(bool active)
    {
        _menu.SetActive(active);
    }
}