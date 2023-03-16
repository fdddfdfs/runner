using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class Localization
{
    public const Languages.Language DefaultLanguage = Languages.Language.English;

    private const string Path = "Localization/";

    private static Localization _instance;

    private Languages.Language _currentLanguage;
    private InGameTextLocalization _defaultLocalization;
    private Dictionary<string, string> _localization;
    private bool _initialized;

    public event Action OnLanguageUpdated;

    public static Localization Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Localization();
                _instance.Initialize();
            }

            return _instance;
        }
    }

    public string this[string key]
    {
        get
        {
            if (_localization.ContainsKey(key))
            {
                return _localization[key];
            }
            
            Debug.LogWarning($"Localization don`t contains translation for key: {key}");

            return "UNLOCALIZED";
        }
    }

    public void ChangeLanguage(Languages.Language newLanguage)
    {
        if (_currentLanguage == newLanguage && _initialized) return;
        
        InGameTextLocalization newLanguageLocalization = LoadLocalization(newLanguage);

        for (var i = 0; i < _defaultLocalization.Texts.Length; i++)
        {
            string text = _defaultLocalization.Texts[i];
            string localizedText = newLanguageLocalization.Texts[i];
            _localization[text] = localizedText;
        }

        _currentLanguage = newLanguage;

        if (_initialized)
        {
            OnLanguageUpdated?.Invoke();
        }
        
        SettingsStorage.Localization.Value = (int)_currentLanguage;
    }

    private static InGameTextLocalization LoadLocalization(Languages.Language language)
    {
        var path = $"{Path}{Languages.SteamJsonLanguages[language]}";
        var targetFile = Resources.Load<TextAsset>(path);

        if (targetFile == null)
        {
            throw new Exception($"Resources doesnt contains localization file {path} for language {language}");
        }
        
        return JsonConvert.DeserializeObject<InGameTextLocalization>(targetFile.text);
    }

    private void Initialize()
    {
        var playerLanguage = (Languages.Language)SettingsStorage.Localization.Value;
        _defaultLocalization = LoadLocalization(DefaultLanguage);
        _localization = new Dictionary<string, string>();
        ChangeLanguage(playerLanguage);
        _initialized = true;
    }
}