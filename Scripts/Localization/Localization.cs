﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class Localization
{
    private const Languages.Language DefaultLanguage = Languages.Language.English;

    private static Localization _instance;
    
    private Languages.Language _currentLanguage;
    private InGameTextLocalization _defaultLocalization;

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

    public Dictionary<string, string> LocalizationDictionary { get; private set; }

    private static InGameTextLocalization LoadLocalization(Languages.Language language)
    {
        string path = Languages.SteamJsonLanguages[language];
        var targetFile = Resources.Load<TextAsset>(path);

        if (targetFile == null)
        {
            throw new Exception($"Resources doesnt contains localization file {path} for language {language}");
        }
        
        return JsonParser.LoadJsonFromFile<InGameTextLocalization>(targetFile.text);
    }

    private void ChangeLanguage(Languages.Language newLanguage)
    {
        if (_currentLanguage == newLanguage) return;
        
        InGameTextLocalization newLanguageLocalization = LoadLocalization(newLanguage);

        for (var i = 0; i < _defaultLocalization.Texts.Length; i++)
        {
            string text = _defaultLocalization.Texts[i];
            string localizedText = newLanguageLocalization.Texts[i];
            LocalizationDictionary[text] = localizedText;
        }

        _currentLanguage = newLanguage;
    }

    private void Initialize()
    {
        _defaultLocalization = LoadLocalization(DefaultLanguage);
        LocalizationDictionary = new Dictionary<string, string>();
        ChangeLanguage(DefaultLanguage);
    }
}