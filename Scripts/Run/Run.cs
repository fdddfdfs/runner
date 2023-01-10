using System.Collections.Generic;
using DG.Tweening;
using StarterAssets;
using UnityEngine;

public sealed class Run : MonoBehaviour, IRunnable
{
    [SerializeField] private Follower _follower;
    [SerializeField] private RunProgress _runProgress;
    [SerializeField] private ThirdPersonController _player;
    [SerializeField] private Map _map;
    [SerializeField] private ResurrectMenu _resurrectMenu;
    [SerializeField] private LoseMenu _loseMenu;
    [SerializeField] private MainMenu _mainMenu;

    private bool _isRun;

    private List<IRunnable> _runnables;
    
    public void StartRun()
    {
        foreach (IRunnable runnable in _runnables)
        {
            runnable.StartRun();
        }

        _isRun = true;
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Lose()
    {
        _map.Level.HideCurrentEnteredBlock();
        
        Time.timeScale = 0;
        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        _resurrectMenu.ShowMenu(_runProgress.Score);
    }

    public void ShowLoseMenu()
    {
        _loseMenu.ShowLoseMenu((int)_runProgress.Score, _runProgress.Money);
    }

    public void Resurrect()
    {
        Time.timeScale = 1;
    }

    public void EndRun()
    {
        // TODO: here will be problem with hidden items
        DOTween.KillAll();
        Coroutines.StopAllRoutines();
        Time.timeScale = 1;

        foreach (IRunnable runnable in _runnables)
        {
            runnable.EndRun();
        }

        _isRun = false;
    }

    public void BackToMenu()
    {
        EndRun();
        _mainMenu.ShowMainMenu();
    }

    private void Update()
    {
        if (_isRun)
        {
            _runProgress.AddScore(Time.deltaTime);
            _runProgress.IncreaseSpeedMultiplayerInTime(Time.deltaTime);
        }
    }

    private void Awake()
    {
        _runnables = new List<IRunnable> { _map, _player, _runProgress, _follower };
    }
}