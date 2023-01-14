using System;

[Flags]
public enum ItemType
{
    Money = 1,
    RandomBoost = 2,
    WeightedRandomBoost = 4,
    HighJump = 8,
    Magnet = 16,
    DoubleMoney = 32,
    Board = 64,
    Immune = 128,
    Fly = 256,
    DoubleScore = 512,
    Spring = 1024,
}
