using UnityEngine;

public class RoadBlock : MonoBehaviour, IMapBlock
{
    public float BlockSize { get; private set; }

    public void Init(float blockSize)
    {
        BlockSize = blockSize;
    }
    
    public void EnterBlock() { }

    public void HideBlock()
    {
        gameObject.SetActive(false);
    }
}