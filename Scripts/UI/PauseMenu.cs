using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _menu;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _backToMenuButton;
    [SerializeField] private Run _run;
    
    public void ChangeMenuActive(bool isActive)
    {
        _menu.SetActive(isActive);
    }

    private void Awake()
    {
        _restartButton.onClick.AddListener(() =>
        {
            _run.ApplyLoseResults();
            _run.EndRun();
            _run.StartRun();
            ChangeMenuActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        });
        
        _backToMenuButton.onClick.AddListener(() =>
        {
            _run.ApplyLoseResults();
            _run.BackToMenu();
            ChangeMenuActive(false);
        });
    }
}