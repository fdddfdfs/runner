using System.Globalization;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class ResurrectMenu : MonoBehaviour
{
    private const int BuyBackPrice = 1000;
    
    private const int SkipTime = 3;
    
    [SerializeField] private GameObject _menu;
    [SerializeField] private Button _buyBack;
    [SerializeField] private Button _skip;
    [SerializeField] private Button _watchAd;
    [SerializeField] private Image _timer;
    [SerializeField] private TMP_Text _buyBackText;
    [SerializeField] private TMP_Text _watchAdText;
    [SerializeField] private TMP_Text _skipText;
    [SerializeField] private TMP_Text _currentScore;
    [SerializeField] private Run _run;

    public void ShowMenu(float score)
    {
        ChangeMenuVisible(true);
        
        ChangeBuybackState();
        _currentScore.text = $"Current score:\n{score.ToString(CultureInfo.InvariantCulture)}";

        _timer.fillAmount = 1;
        _timer.DOFillAmount(0, SkipTime).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() =>
        {
            _run.ShowLoseMenu();
            ChangeMenuVisible(false);
        });
    }

    private void Awake()
    {
        _buyBackText.text = BuyBackPrice.ToString();
        _watchAdText.text = "Watch ad";
        _skipText.text = "Skip";
        
        _buyBack.onClick.AddListener(() =>
        {
            Stats.Money.Value -= BuyBackPrice;
            _run.Resurrect();
            ChangeMenuVisible(false);
            _timer.DOKill();
        });
        
        _skip.onClick.AddListener(() =>
        {
            _run.ShowLoseMenu();
            ChangeMenuVisible(false);
            _timer.DOKill();
        });
        
        _watchAd.onClick.AddListener(() =>
        {
            //TODO: ad
            _run.Resurrect();
            ChangeMenuVisible(false);
            _timer.DOKill();
        });
    }
    
    private void ChangeMenuVisible(bool visible)
    {
        _menu.SetActive(visible);
    }

    private void ChangeBuybackState()
    {
        if (Stats.Money.Value < BuyBackPrice)
        {
            _buyBack.image.color = Color.red;
            _buyBack.enabled = false;
        }
        else
        {
            _buyBack.image.color = Color.green;
            _buyBack.enabled = true;
        }
    }
}