using System;
using System.Collections.Generic;
using System.Threading;
using DG.Tweening;
using StarterAssets;
using UnityEngine;

public sealed class Run : MonoBehaviour, IRunnable, ICancellationTokenProvider
{
    private const int WinRequiredScore = 50;
    private const float FadeMultiplier = 5f;

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
    [SerializeField] private Fade _fade;

    private bool _isRun;
    private List<IRunnable> _runnables;
    private Type _cutsceneType;
    private Type _mainMenuCutsceneType;
    private bool _isCutsceneChanged;

    private CancellationTokenSource _endRunTokenSource = new();
    private CancellationToken[] _linkedTokens;

    private bool _isAlreadyWin;

    public void StartRun()
    {
        if (_endRunTokenSource.IsCancellationRequested)
        {
            _endRunTokenSource?.Dispose();
            _linkedTokens[0] = AsyncUtils.Instance.GetCancellationToken();
            _endRunTokenSource = CancellationTokenSource.CreateLinkedTokenSource(_linkedTokens);
        }

        foreach (IRunnable runnable in _runnables)
        {
            runnable.StartRun();
        }

        _isRun = true;
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        AsyncUtils.TimeScale = 1;
        
        Sounds.Instance.StopAllSounds();
        Sounds.Instance.PlayRandomSounds(2, "Start");
    }

    public void Lose()
    {
        if (!_isRun) return;
        
        _player.Pause();
        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        _resurrectMenu.ShowMenu((int)_runProgress.Score);
        AsyncUtils.TimeScale = 0;
        _pauseController.ChangeAllowingPause(false);
        
        Achievements.Instance.GetAchievement("Lose");
    }

    public void Win()
    {
        _fade.FadeIn(() =>
        {
            _cutscenes.ChangeCurrentCutscene(typeof(WinCutscene));
            _cutscenes.PlayCurrentCutscene();
            _fade.FadeOut(null);
        }, FadeMultiplier);
        
        _map.Level?.HideBlocksBeforePositionZ(_player.transform.position.z + 1000);
        _pauseController.ChangeAllowingPause(false);
        _isRun = false;
        _isAlreadyWin = true;
        _runProgress.ChangeMenuVisible(false);
        //_follower.FollowForTime(_player.gameObject, WinFollowerRunTime);

        ApplyRunResults();
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

    public void ApplyRunResults()
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
        AsyncUtils.TimeScale = 1;
        _map.Level.HideCurrentEnteredBlock();
    }

    public void EndRun()
    {
        DOTween.KillAll();
        Coroutines.StopAllRoutines();

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
            
            Sounds.Instance.PlaySound(2, "BackToMainMenu");
        }
    }

    private void Update()
    {
        if (!_isRun) return;
        
        _runProgress.AddScore(Time.deltaTime * AsyncUtils.TimeScale);
        _runProgress.IncreaseSpeedMultiplayerInTime(Time.deltaTime * AsyncUtils.TimeScale);

        if (!_isAlreadyWin && _runProgress.Score >= WinRequiredScore)
        {
            Win();
        }
    }

    private void Awake()
    {
        _runnables = new List<IRunnable> { _player, _map, _runProgress, _follower, _activeItemsUI, _pauseController };
        SetMainMenuCutscene(typeof(BaseStartCutscene));
    }

    private void OnDisable()
    {
        if (!_endRunTokenSource.IsCancellationRequested)
        {
            _endRunTokenSource.Cancel();
        }
    }

    private void Start()
    {
        _linkedTokens = new[] { AsyncUtils.Instance.GetCancellationToken() };
        _endRunTokenSource = CancellationTokenSource.CreateLinkedTokenSource(_linkedTokens);
    }

    public CancellationToken GetCancellationToken()
    {
        return _endRunTokenSource.Token;
    }
}