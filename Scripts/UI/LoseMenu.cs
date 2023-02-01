using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class LoseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _menu;
    [SerializeField] private TMP_Text _score;
    [SerializeField] private TMP_Text _money;
    [SerializeField] private Button _restart;
    [SerializeField] private Button _backToMenu;
    [SerializeField] private Run _run;


    public void ShowLoseMenu(int score, int money)
    {
        _run.ApplyLoseResults();
        
        ChangeMenuVisible(true);
        _score.text = score.ToString();
        _money.text = $"+{money.ToString()}";
        _restart.Select();

        _restart.onClick.AddListener(() =>
        {
            ChangeMenuVisible(false);
            _run.EndRun();
            _run.StartRun();
        });
        
        _backToMenu.onClick.AddListener(() =>
        {
            ChangeMenuVisible(false);
            _run.BackToMenu();
        });
    }

    private void ChangeMenuVisible(bool visible)
    {
        _menu.SetActive(visible);
    }
}
