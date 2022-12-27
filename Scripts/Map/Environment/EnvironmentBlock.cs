using UnityEngine;

public class EnvironmentBlock : MonoBehaviour, IMapBlock
{
    public void EnterBlock()
    {
        
    }

    public void HideBlock()
    {
        gameObject.SetActive(false);
    }
}