using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class LoseMenu : MonoBehaviour
{
    private const float FadeMultiplier = 5f;
    
    [SerializeField] private GameObject _menu;
    [SerializeField] private TMP_Text _score;
    [SerializeField] private TMP_Text _money;
    [SerializeField] private Button _restart;
    [SerializeField] private Button _backToMenu;
    [SerializeField] private Run _run;
    [SerializeField] private Fade _fade;

    public void ShowLoseMenu(int score, int money)
    {
        _run.ApplyLoseResults();
        
        ChangeMenuVisible(true);
        _score.text = score.ToString();
        _money.text = $"+{money.ToString()}";
        _restart.Select();
    }

    private void ChangeMenuVisible(bool visible)
    {
        _menu.SetActive(visible);
    }

    private void Awake()
    {
        _restart.onClick.AddListener(() =>
        {
            _fade.FadeIn(() =>
            {
                ChangeMenuVisible(false);
                _run.EndRun();
                _run.StartRun();
                _fade.FadeOut(null, FadeMultiplier);
            }, FadeMultiplier);
        });
        
        _backToMenu.onClick.AddListener(() =>
        {
            ChangeMenuVisible(false);
            _run.ShowLoseDecideMenu();
        });
    }
}
