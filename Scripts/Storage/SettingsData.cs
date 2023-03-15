public static class SettingsData
{
    private const float StartVolume = 0.5f;
    private const int DefaultGraphic = 5;

    public static DataFloat SoundVolume { get; } = new (nameof(SoundVolume), StartVolume);

    public static DataFloat MusicVolume { get; } = new(nameof(MusicVolume), StartVolume);

    public static DataInt Graphic { get; } = new(nameof(Graphic), DefaultGraphic);

    public static DataInt Localization { get; } = new(
        nameof(Localization),
        (int)global::Localization.DefaultLanguage);
}