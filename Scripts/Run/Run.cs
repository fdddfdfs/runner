using System;
using DG.Tweening;
using StarterAssets;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class Run : MonoBehaviour
{
    [SerializeField] private RunProgress _runProgress;
    [SerializeField] private ThirdPersonController _player;
    [SerializeField] private Map _map;
    [SerializeField] private ResurrectMenu _resurrectMenu;
    [SerializeField] private LoseMenu _loseMenu;
    [SerializeField] private MainMenu _mainMenu;

    private bool _isRun;
    
    public void StartRun()
    {
        _runProgress.StartRun();
        _player.StartRun();
        _map.StartRun();

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
        
        _player.EndRun();
        _runProgress.EndRun();
        _map.EndRun();

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
}