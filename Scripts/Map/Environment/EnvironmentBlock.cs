public sealed class EnvironmentBlock : Triggerable, IMapBlock
{
    public float BlockSize { get; private set; }

    public void Init(float blockSize)
    {
        BlockSize = blockSize;
        
        base.Init();
    }

    public void EnterBlock() { }

    public void HideBlock()
    {
        gameObject.SetActive(false);
    }
}