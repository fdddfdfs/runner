using System.Threading;
using UnityEngine;

public sealed class Effect : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;

    public async void Activate(int timeMilliseconds, ICancellationTokenProvider cancellationTokenProvider)
    {
        _particleSystem.Play();

        CancellationToken token = cancellationTokenProvider.GetCancellationToken();

        await AsyncUtils.Wait(timeMilliseconds, token);

        if (token.IsCancellationRequested) return;
        
        Destroy(gameObject);
    }
}