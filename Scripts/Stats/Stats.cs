public sealed class Stats
{
    public const int MaxLevel = 10;
    
    private const int StartMoneyValue = 0;
    private const int StartLevel = 1;
    private const int StartBoardsCount = 5;

    public static Stat Money { get; } = new (nameof(Money), StartMoneyValue);
    
    public static Stat HighJumpLevel { get; } = new(nameof(HighJumpLevel), StartLevel);
    
    public static Stat MagnetLevel = new(nameof(MagnetLevel), StartLevel);
    
    public static Stat DoubleMoneyLevel = new(nameof(DoubleMoneyLevel), StartLevel);
    
    public static Stat ImmuneLevel = new(nameof(ImmuneLevel), StartLevel);
    
    public static Stat FlyLevel = new(nameof(FlyLevel), StartLevel);
    
    public static Stat BoardCount = new(nameof(BoardCount), StartBoardsCount);
    
    public static Stat BoardLevel = new(nameof(BoardLevel), StartLevel);
    
    public static Stat DoubleScoreLevel = new(nameof(DoubleScoreLevel), StartLevel);
    
    public static Stat Record = new(nameof(Record), 0);
}