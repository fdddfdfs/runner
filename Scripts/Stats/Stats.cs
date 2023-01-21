public static class Stats
{
    public const int MaxLevel = 10;
    
    private const int StartMoneyValue = 0;
    private const int StartLevel = 1;
    private const int StartBoardsCount = 5;

    public static Stat Money { get; } = new (nameof(Money), StartMoneyValue);
    
    public static Stat HighJumpLevel { get; } = new(nameof(HighJumpLevel), StartLevel);
    
    public static Stat MagnetLevel { get; } = new(nameof(MagnetLevel), StartLevel);
    
    public static Stat DoubleMoneyLevel { get; } = new(nameof(DoubleMoneyLevel), StartLevel);
    
    public static Stat ImmuneLevel { get; } = new(nameof(ImmuneLevel), StartLevel);
    
    public static Stat FlyLevel { get; } = new(nameof(FlyLevel), StartLevel);
    
    public static Stat BoardCount { get; } = new(nameof(BoardCount), StartBoardsCount);
    
    public static Stat BoardLevel { get; } = new(nameof(BoardLevel), StartLevel);
    
    public static Stat DoubleScoreLevel { get; } = new(nameof(DoubleScoreLevel), StartLevel);
    
    public static Stat Record { get; } = new(nameof(Record), 0);
}