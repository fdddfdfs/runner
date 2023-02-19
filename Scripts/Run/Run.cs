using System;
using System.Collections.Generic;
using System.Threading;
using DG.Tweening;
using StarterAssets;
using UnityEngine;

public sealed class Run : MonoBehaviour, IRunnable
{
    [SerializeField] private Follower _follower;
    [SerializeField] private RunProgress _runProgress;
    [SerializeField] private ThirdPersonController _player;
    [SerializeField] private Map _map;
    [SerializeField] private ActiveItemsUI _activeItemsUI;
    [SerializeField] private ResurrectMenu _resurrectMenu;
    [SerializeField] private PauseController _pauseController;
    [SerializeField] private LoseMenu _loseMenu;
    [SerializeField] private MainMenu _mainMenu;
    [SerializeField] private LoseDecideMenu _loseDecideMenu;
    [SerializeField] private Cutscenes _cutscenes;

    private bool _isRun;
    private List<IRunnable> _runnables;
    private Type _cutsceneType;
        
    private CancellationTokenSource _endRunTokenSource = new();

    public CancellationToken EndRunToken => _endRunTokenSource.Token;

    public void StartRun()
    {
        _endRunTokenSource?.Dispose();
        _endRunTokenSource = new CancellationTokenSource();
        
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
        if (_cutsceneType == null)
        {
            _loseDecideMenu.ShowMenu();
            return;
        }
        
        _loseMenu.ShowLoseMenu((int)_runProgress.Score, _runProgress.Money);
    }

    public void SetLoseCutscene(Type type)
    {
        _cutsceneType = type;
    }

    public void ApplyLoseResults()
    {
        DataInt record = Stats.Record;
        _mainMenu.LeaderboardController.UploadResult((int)_runProgress.Score);
        if (record.Value < _runProgress.Score)
        {
            record.Value = (int)_runProgress.Score;
        }

        Stats.Money.Value += _runProgress.Money;
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
        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        foreach (IRunnable runnable in _runnables)
        {
            runnable.EndRun();
        }

        _isRun = false;
        _endRunTokenSource.Cancel();
    }

    public void BackToMenu()
    {
        EndRun();
        _cutscenes.ChangeCurrentCutscene(_cutsceneType);
        _cutscenes.PlayCurrentCutscene();
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
        _runnables = new List<IRunnable> { _player, _map, _runProgress, _follower, _activeItemsUI, _pauseController };
    }

    private void OnDisable()
    {
        if (!_endRunTokenSource.IsCancellationRequested)
        {
            _endRunTokenSource.Cancel();
        }
    }
}