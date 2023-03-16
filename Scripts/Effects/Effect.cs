using UnityEngine;

public sealed class Effect : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;

    public async void Activate(int timeMilliseconds, ICancellationTokenProvider cancellationTokenProvider)
    {
        _particleSystem.Play();

        try
        {
            await AsyncUtils.Wait(timeMilliseconds, cancellationTokenProvider.GetCancellationToken());
        }
        finally
        {
            _particleSystem.Stop();
            gameObject.SetActive(false);
        }
    }
}