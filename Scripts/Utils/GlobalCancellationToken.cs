using System.Threading;
using UnityEngine;

public sealed class GlobalCancellationToken : MonoBehaviour
{
    private readonly CancellationTokenSource _source = new();
    
    public CancellationToken CancellationToken => _source.Token;
    
    public static GlobalCancellationToken Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDisable()
    {
        _source.Cancel();
    }
}