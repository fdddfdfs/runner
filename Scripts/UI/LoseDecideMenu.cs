using System;
using UnityEngine;
using UnityEngine.UI;

public class LoseDecideMenu : MonoBehaviour
{
    [SerializeField] private GameObject _menu;
    [SerializeField] private Button _acceptButton;
    [SerializeField] private Button _declineButton;
    [SerializeField] private Run _run;
    
    public void ShowMenu()
    {
        _menu.SetActive(true);
    }

    private void Awake()
    {
        _acceptButton.onClick.AddListener(() =>
        {
            _run.SetLoseCutscene(typeof(AcceptLoseEndCutscene));
            _run.SetMainMenuCutscene(typeof(AcceptLoseStartCutscene));
            _run.BackToMenu();
            _menu.SetActive(false);
        });
        
        _declineButton.onClick.AddListener(() =>
        {
            _run.SetLoseCutscene(typeof(DeclineLoseEndCutscene));
            _run.SetMainMenuCutscene(typeof(DeclineLoseStartCutscene));
            _run.BackToMenu();
            _menu.SetActive(false);
        });
    }
}