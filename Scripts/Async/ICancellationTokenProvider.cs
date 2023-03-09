using System.Threading;

public interface ICancellationTokenProvider
{
    public CancellationToken GetCancellationToken();
}