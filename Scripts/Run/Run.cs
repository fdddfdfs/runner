using System;
using System.Collections.Generic;
using System.Threading;
using DG.Tweening;
using StarterAssets;
using UnityEngine;

public sealed class Run : MonoBehaviour, IRunnable, ICancellationTokenProvider
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
    private Type _mainMenuCutsceneType;
    private bool _isCutsceneChanged;

    private CancellationTokenSource _endRunTokenSource = new();

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
        _player.Pause();
        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        _resurrectMenu.ShowMenu(_runProgress.Score);
        _pauseController.ChangeAllowingPause(false);
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
        if (_cutsceneType != type)
        {
            _cutsceneType = type;
            _isCutsceneChanged = true;
        }
        else
        {
            _isCutsceneChanged = false;
        }
    }

    public void SetMainMenuCutscene(Type type)
    {
        _mainMenuCutsceneType = type;
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
        _player.UnPause();
        _player.Resurrect();
        _pauseController.ChangeAllowingPause(true);
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

    public void ShowLoseDecideMenu()
    {
        _loseDecideMenu.ShowMenu();
    }

    public void BackToMenu(bool playCutscene = true)
    {
        EndRun();

        if (playCutscene && _isCutsceneChanged)
        {
            _cutscenes.ChangeCurrentCutscene(_cutsceneType);
            _cutscenes.PlayCurrentCutscene();
        }
        else
        {
            _mainMenu.SetCutsceneType(_mainMenuCutsceneType);
            _mainMenu.ShowMainMenu();
        }
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

    public CancellationToken GetCancellationToken()
    {
        return _endRunTokenSource.Token;
    }
}