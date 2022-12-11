using DG.Tweening;
using StarterAssets;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class Run : MonoBehaviour
{
    [SerializeField] private RunProgress _runProgress;
    [SerializeField] private ThirdPersonController _player;
    [SerializeField] private Level _level;
    [SerializeField] private ResurrectMenu _resurrectMenu;

    public void StartRun()
    {
        _runProgress.StartRun();
        _player.StartRun();
        _level.StartRun();
    }

    public void Lose()
    {
        Time.timeScale = 0;
        
        _resurrectMenu.ShowMenu(_runProgress.Score);
    }

    public void Resurrect()
    {
        Time.timeScale = 1;
        _level.HideCurrentBlock();
    }

    public void EndRun()
    {
        // TODO: here will be problem with hidden items
        DOTween.KillAll();
        Coroutines.StopAllRoutines();
        Time.timeScale = 1;
        
        _runProgress.EndRun();
        _player.EndRun();
        _level.EndRun();
    }
}