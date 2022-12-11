using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public sealed class RunProgress : MonoBehaviour
{
    public const int DefaultMoneyMultiplier = 1;
    
    [SerializeField] private TMPro.TMP_Text _scoreText;
    [SerializeField] private TMPro.TMP_Text _moneyText;

    private float _score;
    private int _money;
    private int _moneyMultiplier = DefaultMoneyMultiplier;

    public float Score => _score;

    public void AddScore(float value)
    {
        _score += value;
        _scoreText.text = _score.ToString(CultureInfo.InvariantCulture);
    }

    public void AddMoney(int money = 1)
    {
        _money += money * _moneyMultiplier;
        _moneyText.text = _money.ToString(CultureInfo.InvariantCulture);
    }

    public void ChangeMoneyMultiplier(int multiplier = DefaultMoneyMultiplier)
    {
        _moneyMultiplier = multiplier;
    }

    public void StartRun()
    {
        _money = 0;
        _moneyText.text = _money.ToString(CultureInfo.InvariantCulture);
        _score = 0;
        _scoreText.text = _score.ToString(CultureInfo.InvariantCulture);
        
        ChangeMenuVisible(true);
        
        ChangeMoneyMultiplier();
    }

    public void EndRun()
    {
        Stats.Instance.Money += _money;

        ChangeMenuVisible(false);
        
        _money = 0;
        _score = 0;
    }

    private void Awake()
    {
        ChangeMenuVisible(false);
    }

    private void ChangeMenuVisible(bool visible)
    {
        _scoreText.gameObject.SetActive(visible);
        _moneyText.gameObject.SetActive(visible);
    }
}
