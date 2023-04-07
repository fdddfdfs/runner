using System;
using StarterAssets;
using UnityEngine;
using UnityEngine.UI;

public sealed class LoseDecideMenu : MonoBehaviour
{
    [SerializeField] private GameObject _menu;
    [SerializeField] private Button _acceptButton;
    [SerializeField] private Button _declineButton;
    [SerializeField] private ThirdPersonController _player;
    [SerializeField] private Run _run;
    [SerializeField] private Map _map;

    private Type _endCutscene;
    private Type _startCutscene;
    
    public void ShowMenu()
    {
        _menu.SetActive(true);
    }

    public void BackToMainMenu()
    {
        _run.SetLoseCutscene(_endCutscene);
        _run.SetMainMenuCutscene(_startCutscene);
        _run.ApplyLoseResults();
        _run.BackToMenu();
    }

    private void Awake()
    {
        _acceptButton.onClick.AddListener(() =>
        {
            _endCutscene = typeof(AcceptLoseEndCutscene);
            _startCutscene = typeof(AcceptLoseStartCutscene);
            _menu.SetActive(false);
            _player.Lose();
            _map.ChangeStartLocationDetailsDensity(1);
            
            Sounds.Instance.PlayRandomSounds(2,"Sign");
        });
        
        _declineButton.onClick.AddListener(() =>
        {
            _endCutscene = typeof(DeclineLoseEndCutscene);
            _startCutscene = typeof(DeclineLoseStartCutscene);
            _menu.SetActive(false);
            _player.Lose();
            _map.ChangeStartLocationDetailsDensity(0);
            
            Sounds.Instance.PlayRandomSounds(2,"Throw");
        });
    }
}