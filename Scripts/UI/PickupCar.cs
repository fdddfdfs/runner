using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class PickupCar : Menu, IRunnable
{
    private const float FadeTime = 0.5f;
    
    private readonly Color _startColor = new(1, 1, 1);
    private readonly Color _endColor = new(1,187/255f,0);
    
    [SerializeField] private List<TMP_Text> _texts;
    [SerializeField] private TMP_Text _count;
    
    private bool _isActivated;

    public void ShowPickupCar()
    {
        if (_menu.activeSelf)
        {
            _count.text = $"X{Stats.BoardCount.Value}";
        }
        
        if (_isActivated) return;

        _isActivated = true;
        
        ChangeMenuActive(true);

        _count.text = $"X{Stats.BoardCount.Value}";

        foreach (TMP_Text text in _texts)
        {
            text.color = _startColor;
            text.DOColor(_endColor, FadeTime).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        }
    }

    public void HidePickupCar()
    {
        if (!_menu.activeSelf) return;
        
        foreach (TMP_Text text in _texts)
        {
            text.DOKill();
        }

        ChangeMenuActive(false);
    }

    public void StartRun()
    {
        _isActivated = false;
    }

    public void EndRun()
    {
        HidePickupCar();
    }
}