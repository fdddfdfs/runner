using UnityEngine;

public sealed class EnvironmentBlock : MonoBehaviour, IMapBlock
{
    private float _blockSize;
    
    public float BlockSize => _blockSize;

    public void Init(float blockSize)
    {
        _blockSize = blockSize;
    }
    
    public void EnterBlock() { }

    public void HideBlock()
    {
        gameObject.SetActive(false);
    }
}