using System.Collections.Generic;
using UnityEngine;

public sealed class PauseController : MonoBehaviour, IPauseable, IRunnable
{
    [SerializeField] private PauseMenu _pauseMenu;
    [SerializeField] private List<GameObject> _pauseableObjects;

    private List<IPauseable> _pausables;
    private bool _isPause;
    private bool _isRun;
    
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
    
    public void ChangePauseState()
    {
        if (_isPause)
        {
            UnPause();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Pause();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        _isPause = !_isPause;
        
        _pauseMenu.ChangeMenuActive(_isPause);
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
        if (Input.GetKeyDown(KeyCode.Escape) && _isRun)
        {
            ChangePauseState();
        }
    }

    public void StartRun()
    {
        _isRun = true;
    }

    public void EndRun()
    {
        _isRun = false;
    }
}
