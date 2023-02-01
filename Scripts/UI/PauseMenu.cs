using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _menu;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _backToMenuButton;
    [SerializeField] private Run _run;
    [SerializeField] private PauseController _pauseController;

    public void ChangeMenuActive(bool isActive)
    {
        _menu.SetActive(isActive);

        if (isActive)
        {
            _restartButton.Select();
        }
    }

    private void Awake()
    {
        _restartButton.onClick.AddListener(() =>
        {
            _pauseController.ChangePauseState();
            
            _run.ApplyLoseResults();
            _run.EndRun();
            _run.StartRun();
        });
        
        _backToMenuButton.onClick.AddListener(() =>
        {
            _pauseController.ChangePauseState();
            
            _run.ApplyLoseResults();
            _run.BackToMenu();
        });
    }
}