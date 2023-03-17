public sealed class EnvironmentBlock : Triggerable, IMapBlock
{
    public float BlockSize { get; private set; }

    public void Init(float blockSize)
    {
        BlockSize = blockSize;
        
        base.Init();
    }

    public void EnterBlock() { }

    public async void HideBlock()
    {
        await AsyncUtils.Wait(0.1f, AsyncUtils.Instance.GetCancellationToken());
        
        gameObject.SetActive(false);
    }
}