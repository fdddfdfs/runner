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

        _steamJsonLanguages = new Dictionary<Language, string>();
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
        Arabic,
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
    
    public static IReadOnlyDictionary<Language, string> SteamJsonLanguages => _steamJsonLanguages;

    private static readonly Dictionary<Language, string> _steamJsonLanguages;
}
