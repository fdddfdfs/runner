using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PauseController : MonoBehaviour, IPauseable
{
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private List<GameObject> _pauseableObjects;

    private List<IPauseable> _pausables;
    private bool _isPause;
    
    public void Pause()
    {
        foreach (var pauseable in _pausables)
        {
            pauseable.Pause();
        }
    }

    public void UnPause()
    {
        foreach (var pauseable in _pausables)
        {
            pauseable.UnPause();
        }
    }

    private void Awake()
    {
        _pausables = new List<IPauseable>();
        foreach (var pauseableObject in _pauseableObjects)
        {
            _pausables.Add(pauseableObject.GetComponent<IPauseable>());
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ChangePauseState();
        }
    }

    private void ChangePauseState()
    {
        if (_isPause)
        {
            UnPause();
        }
        else
        {
            Pause();
        }

        _isPause = !_isPause;
        
        _pauseMenu.SetActive(_isPause);
    }
}
