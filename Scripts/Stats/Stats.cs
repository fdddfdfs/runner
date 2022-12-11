using UnityEngine;
public class Stats : MonoBehaviour
{
    public const int MaxLevel = 10;
    
    private const int StartMoneyValue = 0;
    private const int StartLevel = 1;
    private const int StartBoardsCount = 5;

    public static Stats Instance;

    private int _money;
    private int _highJumpLevel;
    private int _magnetLevel;
    private int _doubleMoneyLevel;
    private int _immuneLevel;
    private int _flyLevel;
    private int _boardCount;

    public int Money
    {
        get => _money;
        set
        {
            _money = value;
            Prefs.SaveVariable(_money, nameof(_money));
        }
    }
    
    public int HighJumpLevel
    {
        get => _magnetLevel;
        set
        {
            _highJumpLevel = value;
            Prefs.SaveVariable(_highJumpLevel, nameof(_highJumpLevel));
        }
    }
    
    public int MagnetLevel
    {
        get => _magnetLevel;
        set
        {
            _highJumpLevel = value;
            Prefs.SaveVariable(_magnetLevel, nameof(_magnetLevel));
        }
    }

    public int ImmuneLevel
    {
        get => _immuneLevel;
        set
        {
            _immuneLevel = value;
            Prefs.SaveVariable(_immuneLevel, nameof(_immuneLevel));
        }
    }

    public int DoubleMoneyLevel
    {
        get => _doubleMoneyLevel;
        set
        {
            _doubleMoneyLevel = value;
            Prefs.SaveVariable(_doubleMoneyLevel, nameof(_doubleMoneyLevel));
        }
    }
    
    public int FlyLevel
    {
        get => _flyLevel;
        set
        {
            _flyLevel = value;
            Prefs.SaveVariable(_flyLevel, nameof(_flyLevel));
        }
    }

    public int BoardCount
    {
        get => _boardCount;
        set
        {
            _boardCount = value;
            Prefs.SaveVariable(_boardCount, nameof(_boardCount));
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Initialize()
    {
        Prefs.LoadVariable(out _money, nameof(_money), StartMoneyValue);
        Prefs.LoadVariable(out _highJumpLevel, nameof(_highJumpLevel), StartLevel);
        Prefs.LoadVariable(out _magnetLevel, nameof(_magnetLevel), StartLevel);
        Prefs.LoadVariable(out _doubleMoneyLevel, nameof(_doubleMoneyLevel), StartLevel);
        Prefs.LoadVariable(out _immuneLevel, nameof(_immuneLevel), StartLevel);
        Prefs.LoadVariable(out _flyLevel, nameof(_flyLevel), StartLevel);
        Prefs.LoadVariable(out _boardCount, nameof(_boardCount), StartBoardsCount);
    }
}