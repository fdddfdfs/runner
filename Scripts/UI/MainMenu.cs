using TMPro;
using UnityEngine;

public sealed class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text _startText;
    [SerializeField] private UpgradeMenu _upgradeMenu;
    [SerializeField] private StatsMenu _statsMenu;
    [SerializeField] private Run _run;

    private bool _isRun;

    public void ShowMainMenu()
    {
        _upgradeMenu.ChangeMenuVisible(true);
        _startText.gameObject.SetActive(true);
        _statsMenu.ChangeActiveState(true);
        _isRun = false;
    }

    private void Awake()
    {
        _startText.text = "Press F to start run";
        ShowMainMenu();
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

        _startText.gameObject.SetActive(false);
        _isRun = true;
        
        _run.StartRun();
    }
}