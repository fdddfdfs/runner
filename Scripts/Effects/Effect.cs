using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Effect : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;

    public async void Activate(int timeMilliseconds, ICancellationTokenProvider cancellationTokenProvider)
    {
        _particleSystem.Play();

        try
        {
            await Task.Delay(timeMilliseconds, cancellationTokenProvider.GetCancellationToken());
        }
        finally
        {
            _particleSystem.Stop();
            gameObject.SetActive(false);
        }
    }
}