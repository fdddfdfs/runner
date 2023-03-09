using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public sealed class AsyncUtils : MonoBehaviour, ICancellationTokenProvider
{
    private CancellationTokenSource _source;
    private static AsyncUtils _instance;

    public static AsyncUtils Instance
    {
        get
        {
            if (!_instance)
            {
                var gameObject = new GameObject(nameof(AsyncUtils));
                DontDestroyOnLoad(gameObject);
                _instance = gameObject.AddComponent<AsyncUtils>();
                _instance._source = new CancellationTokenSource();
            }

            return _instance;
        }
    }
    
    public static async Task Wait(float time, bool unscaledTime = false)
    {
        float currentTime = Time.time;
        float targetTime = currentTime + time;
        while (currentTime < targetTime)
        {
            currentTime += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            await Task.Yield();
        }
    }
    
    public static async Task Wait(float time, CancellationToken cancellationToken, bool unscaledTime = false)
    {
        float currentTime = Time.time;
        float targetTime = currentTime + time;
        while (currentTime < targetTime)
        {
            currentTime += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            await Task.Yield();

            if (cancellationToken.IsCancellationRequested) return;
        }
    }

    public static Action<Task> EmptyTask => _ => { };

    private void OnDisable()
    {
        _source.Cancel();
    }

    public CancellationToken GetCancellationToken()
    {
        return _source.Token;
    }
}