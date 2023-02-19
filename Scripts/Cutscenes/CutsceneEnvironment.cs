using UnityEngine;

public class CutsceneEnvironment : MonoBehaviour
{
    [SerializeField] private GameObject _parent;

    public void ChangeEnvironmentActive(bool active)
    {
        _parent.SetActive(active);
    }
}