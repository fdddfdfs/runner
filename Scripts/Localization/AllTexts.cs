using System.Collections.Generic;

public static class AllTexts
{
    public static string OpenChest => "Open Chest";
    public static string PutClothes => "Change Clothes";
    public static string Music => "Music";
    public static string Sounds => "Sounds";
    public static string Graphic => "Graphic";
    public static string Language => "Language";

    public static Dictionary<GraphicQuality, string> GraphicQualities { get; } = new()
    {
        [GraphicQuality.VeryLow] = "Very Low",
        [GraphicQuality.Low] = "Low",
        [GraphicQuality.Medium] = "Medium",
        [GraphicQuality.High] = "High",
        [GraphicQuality.VeryHigh] = "Very High",
        [GraphicQuality.Ultra] = "Ultra",
    };

    public static string StartRun => "Press F to start run";

    public static string CurrentScore => "Current score:";

    public static string Skip => "Skip";
}