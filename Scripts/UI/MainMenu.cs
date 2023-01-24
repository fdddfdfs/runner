using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text _startText;
    [SerializeField] private UpgradeMenu _upgradeMenu;
    [SerializeField] private StatsMenu _statsMenu;
    [SerializeField] private Run _run;
    [SerializeField] private Button _exit;

    private bool _isRun;

    public void ShowMainMenu()
    {
        _upgradeMenu.ChangeMenuVisible(true);
        _startText.gameObject.SetActive(true);
        _statsMenu.ChangeActiveState(true);
        _isRun = false;
        _exit.gameObject.SetActive(true);
    }

    private void Awake()
    {
        _startText.text = "Press F to start run";
        ShowMainMenu();
        
        _exit.onClick.AddListener(Application.Quit);
    }

    private void Update()
    {
        if (_isRun) return;
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartRun();
        }
    }

    private void StartRun()
    {
        _upgradeMenu.ChangeMenuVisible(false);
        _statsMenu.ChangeActiveState(false);
        _exit.gameObject.SetActive(false);

        _startText.gameObject.SetActive(false);
        _isRun = true;
        
        _run.StartRun();
    }
}