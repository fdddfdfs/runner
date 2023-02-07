﻿using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public sealed class GlobalCancellationToken : MonoBehaviour
{
    private CancellationTokenSource _source;
    private static GlobalCancellationToken _instance;
    
    public CancellationToken CancellationToken => _source.Token;

    public static GlobalCancellationToken Instance
    {
        get
        {
            if (!_instance)
            {
                var gameObject = new GameObject(nameof(GlobalCancellationToken));
                DontDestroyOnLoad(gameObject);
                _instance = gameObject.AddComponent<GlobalCancellationToken>();
                _instance._source = new CancellationTokenSource();
            }

            return _instance;
        }
    }

    public static Action<Task> EmptyTask => _ => { };

    private void OnDisable()
    {
        _source.Cancel();
    }
}