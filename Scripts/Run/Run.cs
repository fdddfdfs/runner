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
    
    public void StartRun()
    {
        _runProgress.StartRun();
        _player.StartRun();
        _map.StartRun();
    }

    public void Lose()
    {
        Time.timeScale = 0;
        
        _resurrectMenu.ShowMenu(_runProgress.Score);
    }

    public void ShowLoseMenu()
    {
        _loseMenu.ShowLoseMenu((int)_runProgress.Score, _runProgress.Money);
    }

    public void Resurrect()
    {
        Time.timeScale = 1;
        _map.HideCurrentBlock();
    }

    public void EndRun()
    {
        // TODO: here will be problem with hidden items
        DOTween.KillAll();
        Coroutines.StopAllRoutines();
        Time.timeScale = 1;
        
        _runProgress.EndRun();
        _player.EndRun();
        _map.EndRun();
    }

    public void BackToMenu()
    {
        EndRun();
        _mainMenu.ShowMainMenu();
    }
}