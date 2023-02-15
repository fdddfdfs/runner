﻿using fdddfdfs.Leaderboard;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public sealed class MainMenu : MonoBehaviour
{
    [SerializeField] private ChangeTextOnStart _startTextChangeTextOnStart;
    [SerializeField] private TMP_Text _startText;
    [SerializeField] private StatsMenu _statsMenu;
    [SerializeField] private Button _exit;
    [SerializeField] private InputActionAsset _inputActionAsset;
    [SerializeField] private MainMenuRightMenu _rightMenu;
    [SerializeField] private Cutscenes _cutscenes;

    private bool _isStartPressed;

    private bool _isRun;

    public LeaderboardController LeaderboardController => _rightMenu.LeaderboardController;

    public void ShowMainMenu()
    {
        _rightMenu.ChangeMenuActive(true);
        _startTextChangeTextOnStart.EndRun();
        _statsMenu.ChangeActiveState(true);
        _isRun = false;
        _exit.gameObject.SetActive(true);
    }

    private void Start()
    {
        _startText.text = "Press F/Start to start run";
        ShowMainMenu();
        
        _exit.onClick.AddListener(Application.Quit);

        InputActionMap inputActionMap = _inputActionAsset.FindActionMap("UI", true);
        inputActionMap.Enable();
        inputActionMap["StartRace"].started += (_) => _isStartPressed = true;
        inputActionMap["StartRace"].canceled += (_) => _isStartPressed = false;
    }

    private void Update()
    {
        if (_isRun) return;
        
        if (_isStartPressed)
        {
            StartRun();
        }
    }

    private void StartRun()
    {
        _rightMenu.ChangeMenuActive(false);
        _statsMenu.ChangeActiveState(false);
        _exit.gameObject.SetActive(false);
        _startTextChangeTextOnStart.StartRun();
        _isRun = true;
        
        _cutscenes.PlayCurrentCutscene();
    }
}