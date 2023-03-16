using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class PauseController : MonoBehaviour, IPauseable, IRunnable
{
    [SerializeField] private PauseMenu _pauseMenu;
    [SerializeField] private List<GameObject> _pauseableObjects;
    [SerializeField] private InputActionAsset _inputActionAsset;

    private List<IPauseable> _pausables;
    private bool _isPause;
    private bool _isRun;
    private bool _isPausePressed;
    private bool _isPauseAllowed;

    public void ChangeAllowingPause(bool isPauseAllowed)
    {
        _isPauseAllowed = isPauseAllowed;
    }
    
    public void Pause()
    {
        foreach (IPauseable pauseable in _pausables)
        {
            pauseable.Pause();
        }
    }

    public void UnPause()
    {
        foreach (IPauseable pauseable in _pausables)
        {
            pauseable.UnPause();
        }
    }
    
    public void ChangePauseState()
    {
        if (!_isPauseAllowed) return;
        
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

        InputActionMap inputActionMap = _inputActionAsset.FindActionMap("Player", true);
        inputActionMap["Pause"].started += (_) =>
        {
            if (_isRun)
            {
                _isPausePressed = true;
            }
        };
    }

    private void Update()
    {
        if (_isPausePressed && _isRun)
        {
            _isPausePressed = false;
            ChangePauseState();
        }
    }

    public void StartRun()
    {
        _isRun = true;
        _isPauseAllowed = true;
    }

    public void EndRun()
    {
        _isRun = false;
        _isPauseAllowed = false;
    }
}
