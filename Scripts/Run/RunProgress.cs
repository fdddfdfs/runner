using System.Globalization;
using Steamworks;
using UnityEngine;

public sealed class RunProgress : MonoBehaviour, IRunnable
{
    public const int DefaultMoneyMultiplier = 1;
    public const int DefaultScoreMultiplier = 10;
    private const float DefaultSpeedMultiplier = 1;
    private const int MoneyAchievementsNumber = 9;
    private const int ScoreAchievementsNumber = 9;
    private const int MoneyToPlaySound = 250;

    [SerializeField] private GameObject _scoreParent;
    [SerializeField] private GameObject _moneyParent;
    [SerializeField] private ShowTextOnStart _scoreShowText;
    [SerializeField] private ShowTextOnStart _moneyShowText;
    [SerializeField] private MainMenuRightMenu _mainMenuRightMenu;

    private int _moneyMultiplier = DefaultMoneyMultiplier;
    private int _scoreMultiplier = DefaultScoreMultiplier;

    private string[] _moneyAchievements;
    private string[] _scoreAchievements;

    private float _runTime;
    private float _scoreDelta;

    public float SpeedMultiplier { get; private set; } = DefaultSpeedMultiplier;

    public float HalfSpeedMultiplier { get; private set; } = DefaultSpeedMultiplier;

    public float Score { get; private set; }

    public int Money { get; private set; }

    public void AddScore(float value)
    {
        float addedValue = value * _scoreMultiplier * SpeedMultiplier;
        Score += addedValue;
        _scoreShowText.Text.text = ((int)Score).ToString(CultureInfo.InvariantCulture);

        _scoreDelta += addedValue;

        if (_scoreDelta > 1)
        {
            for (var i = 0; i < _scoreAchievements.Length; i++)
            {
                Achievements.Instance.IncreaseProgress(_scoreAchievements[i], 1);
            }

            _scoreDelta -= 1;
        }
    }

    public void AddMoney(int money = 1)
    {
        int addedMoney = money * _moneyMultiplier;
        
        Money += addedMoney;
        _moneyShowText.Text.text = Money.ToString(CultureInfo.InvariantCulture);

        for (var i = 0; i < _moneyAchievements.Length; i++)
        {
            Achievements.Instance.IncreaseProgress(_moneyAchievements[i], addedMoney);
        }

        if (Money % MoneyToPlaySound == 0)
        {
            Sounds.Instance.PlayRandomSounds(2, "Money");
        }
    }

    public void IncreaseSpeedMultiplayerInTime(float time)
    {
        if (time == 0) return;

        _runTime += time;
        SpeedMultiplier = SpeedMultiplayerFunc(_runTime);
        HalfSpeedMultiplier = SpeedMultiplier / 2;
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
        _runTime = 0;

        if (SteamManager.Initialized)
        {
            SteamInventory.TriggerItemDrop(
                out SteamInventoryResult_t result,
                new SteamItemDef_t(InventoryAllItems.PlaytimeGeneratorID));
            
            _mainMenuRightMenu.SetInventoryResult(result);
        }

        foreach (string scoreAchievement in _scoreAchievements)
        {
            Achievements.Instance.ResetProgress(scoreAchievement);
        }
    }
    
    public void ChangeMenuVisible(bool visible)
    {
        _moneyParent.SetActive(visible);
        _scoreParent.SetActive(visible);
        
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

    private void Awake()
    {
        _moneyAchievements = new string[MoneyAchievementsNumber];
        for (int i = 0; i < MoneyAchievementsNumber; i++)
        {
            _moneyAchievements[i] = $"Money_{(i + 1).ToString()}";
        }

        _scoreAchievements = new string[ScoreAchievementsNumber];
        for (int i = 0; i < ScoreAchievementsNumber; i++)
        {
            _scoreAchievements[i] = $"Score_{(i + 1).ToString()}";
        }
    }

    private void Start()
    {
        _scoreShowText.SetDilate(-1);
        _moneyShowText.SetDilate(-1);

        _moneyParent.SetActive(false);
        _scoreParent.SetActive(false);
    }

    private static float SpeedMultiplayerFunc(float time)
    {
        float speedMultiplayer = 1 * Mathf.Log(time / 15f);
        speedMultiplayer = 2f + (speedMultiplayer < 0 ? 0 : speedMultiplayer);
        return speedMultiplayer;
    }
}
