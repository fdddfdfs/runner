using System;
using System.Collections.Generic;
using System.Linq;

public static class Languages
{
    static Languages()
    {
        var inGameLanguages = (Language[])Enum.GetValues(typeof(Language));
        string[] inGameLanguageNames = Enum.GetNames(typeof(Language));

        IEnumerable<(Language Language, string Name)> languages =
            inGameLanguages.Zip(inGameLanguageNames, (language, name) => (language, name));

        foreach ((Language Language, string Name) language in languages)
        {
            if (!_steamJsonLanguages.ContainsKey(language.Language))
            {
                _steamJsonLanguages[language.Language] = language.Name;
            }
        }
    }

    public enum Language
    {
        English,
        German,
        French,
        Italian,
        Korean,
        SpanishSpain,
        SimplifiedChinese,
        TraditionalChinese,
        Russian,
        Thai,
        Japanese,
        PortuguesePortugal,
        Polish,
        Danish,
        Dutch,
        Finnish,
        Norwegian,
        Swedish,
        Hungarian,
        Czech,
        Romanian,
        Turkish,
        PortugueseBrazil,
        Bulgarian,
        Greek,
        Ukrainian,
        Vietnamese,
        SpanishLatinAmerica,
        SteamChina,
        Afrikaans,
        Amharic,
        Assamese,
        Bangla,
        Belarusian,
        Catalan,
        Estonian,
        Galician,
        Gujarati,
        Hebrew,
        Icelandic,
        Indonesian,
        Kannada,
        Khmer,
        Kyrgyz,
        Luxembourgish,
        Malay,
        Maltese,
        Marathi,
        Nepali,
        PunjabiGurmukhi,
        Quechua,
        Serbian,
        Sinhala,
        Slovenian,
        Sotho,
        Tajik,
        Tatar,
        Tigrinya,
        Urdu,
        Uzbek,
        Xhosa,
        Zulu,
        Albanian,
        Armenian,
        Azerbaijani,
        Basque,
        Bosnian,
        Croatian,
        Filipino,
        Georgian,
        Hausa,
        Hindi,
        Igbo,
        Irish,
        Kazakh,
        Konkani,
        Lithuanian,
        Macedonian,
        Malayalam,
        Maori,
        Mongolian,
        Persian,
        PunjabiShahmukhi,
        Scots,
        Sindhi,
        Slovak,
        Sorani,
        Swahili,
        Tamil,
        Telugu,
        Turkmen,
        Uyghur,
        Welsh,
        Yoruba,
        Kinyarwanda,
        Latvian,
        Odia,
    }

    private static readonly Dictionary<Language, string> _steamJsonLanguages = new()
    {
        { Language.English, "english" },
        { Language.German, "german" },
        { Language.French, "french" },
        { Language.Italian, "italian" },
        { Language.Korean, "koreana" },
        { Language.SpanishSpain, "spanish" },
        { Language.SimplifiedChinese, "schinese" },
        { Language.TraditionalChinese, "tchinese" },
        { Language.Russian, "russian" },
        { Language.Thai, "thai" },
        { Language.Japanese, "japanese" },
        { Language.PortuguesePortugal, "portuguese" },
        { Language.Polish, "polish" },
        { Language.Danish, "danish" },
        { Language.Dutch, "dutch" },
        { Language.Finnish, "finnish" },
        { Language.Norwegian, "norwegian" },
        { Language.Swedish, "swedish" },
        { Language.Hungarian, "hungarian" },
        { Language.Czech, "czech" },
        { Language.Romanian, "romanian" },
        { Language.Turkish, "turkish" },
        { Language.PortugueseBrazil, "brazilian" },
        { Language.Bulgarian, "bulgarian" },
        { Language.Greek, "greek" },
        { Language.Ukrainian, "ukrainian" },
        { Language.Vietnamese, "vietnamese" },
        { Language.SpanishLatinAmerica, "latam" },
        { Language.SteamChina, "sc_schinese" },
    };

    public static IReadOnlyDictionary<Language, string> SteamJsonLanguages => _steamJsonLanguages;
}
