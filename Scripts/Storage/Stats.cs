public static class Stats
{
    public const int MaxLevel = 10;
    
    private const int StartMoneyValue = 0;
    private const int StartLevel = 1;
    private const int StartBoardsCount = 5;

    public static DataInt Money { get; } = new (nameof(Money), StartMoneyValue);
    
    public static DataInt HighJumpLevel { get; } = new(nameof(HighJumpLevel), StartLevel);
    
    public static DataInt MagnetLevel { get; } = new(nameof(MagnetLevel), StartLevel);
    
    public static DataInt DoubleMoneyLevel { get; } = new(nameof(DoubleMoneyLevel), StartLevel);
    
    public static DataInt ImmuneLevel { get; } = new(nameof(ImmuneLevel), StartLevel);
    
    public static DataInt FlyLevel { get; } = new(nameof(FlyLevel), StartLevel);
    
    public static DataInt BoardCount { get; } = new(nameof(BoardCount), StartBoardsCount);
    
    public static DataInt BoardLevel { get; } = new(nameof(BoardLevel), StartLevel);
    
    public static DataInt DoubleScoreLevel { get; } = new(nameof(DoubleScoreLevel), StartLevel);
    
    public static DataInt Record { get; } = new(nameof(Record), 0);
}