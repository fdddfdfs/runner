using System.Globalization;
using UnityEngine;

public sealed class RunProgress : MonoBehaviour, IRunnable
{
    public const int DefaultMoneyMultiplier = 1;
    public const int DefaultScoreMultiplier = 1;
    private const float DefaultSpeedMultiplier = 1;
    
    [SerializeField] private ShowTextOnStart _scoreShowText;
    [SerializeField] private ShowTextOnStart _moneyShowText;

    private int _moneyMultiplier = DefaultMoneyMultiplier;
    private int _scoreMultiplier = DefaultScoreMultiplier;

    public float SpeedMultiplier { get; private set; } = DefaultSpeedMultiplier;

    public float HalfSpeedMultiplier { get; private set; } = DefaultSpeedMultiplier;

    public float Score { get; private set; }

    public int Money { get; private set; }

    public void AddScore(float value)
    {
        Score += value * _scoreMultiplier * SpeedMultiplier;
        _scoreShowText.Text.text =((int)Score).ToString(CultureInfo.InvariantCulture);
    }
    
    public void AddMoney(int money = 1)
    {
        Money += money * _moneyMultiplier;
        _moneyShowText.Text.text = Money.ToString(CultureInfo.InvariantCulture);
    }

    public void IncreaseSpeedMultiplayerInTime(float time)
    {
        float delta = SpeedMultiplayerFunc(time);
        SpeedMultiplier += delta;
        HalfSpeedMultiplier += delta / 2;
    }

    public void ChangeMoneyMultiplier(int multiplier = DefaultMoneyMultiplier)
    {
        _moneyMultiplier = multiplier;
    }

    public void ChangeScoreMultiplier(int multiplier = DefaultScoreMultiplier)
    {
        _scoreMultiplier = multiplier;
    }

    public void StartRun()
    {
        Money = 0;
        _moneyShowText.Text.text = Money.ToString(CultureInfo.InvariantCulture);
        Score = 0;
        _scoreShowText.Text.text = ((int)Score).ToString(CultureInfo.InvariantCulture);
        
        ChangeMenuVisible(true);
        
        ChangeMoneyMultiplier();
    }

    public void EndRun()
    {
        ChangeMenuVisible(false);
        
        Money = 0;
        Score = 0;
        SpeedMultiplier = DefaultSpeedMultiplier;
        HalfSpeedMultiplier = DefaultSpeedMultiplier;
        _scoreMultiplier = DefaultScoreMultiplier;
        _moneyMultiplier = DefaultMoneyMultiplier;
    }

    private void Awake()
    {
        _scoreShowText.SetDilate(-1);
        _moneyShowText.SetDilate(-1);
    }

    private void ChangeMenuVisible(bool visible)
    {
        if (visible)
        {
            _scoreShowText.StartRun();
            _moneyShowText.StartRun();
        }
        else
        {
            _scoreShowText.EndRun();
            _moneyShowText.EndRun();
        }
    }
    
    private static float SpeedMultiplayerFunc(float time)
    {
        return time / 60;
    }
}
