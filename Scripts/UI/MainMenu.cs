using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public sealed class MainMenu : MonoBehaviour
{
    [SerializeField] private ShowFont _startTextShowFont;
    [SerializeField] private TMP_Text _startText;
    [SerializeField] private UpgradeMenu _upgradeMenu;
    [SerializeField] private StatsMenu _statsMenu;
    [SerializeField] private Run _run;
    [SerializeField] private Button _exit;
    [SerializeField] private InputActionAsset _inputActionAsset;

    private bool _isStartPressed;

    private bool _isRun;

    public void ShowMainMenu()
    {
        _upgradeMenu.ChangeMenuVisible(true);
        _startTextShowFont.EndRun();
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
        _upgradeMenu.ChangeMenuVisible(false);
        _statsMenu.ChangeActiveState(false);
        _exit.gameObject.SetActive(false);
        _startTextShowFont.StartRun();
        _isRun = true;
        
        _run.StartRun();
    }
}