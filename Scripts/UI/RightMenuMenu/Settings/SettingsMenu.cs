using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class SettingsMenu : Menu
{
    [SerializeField] private TMP_Text _musicHeader;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private TMP_Text _musicValue;
    [SerializeField] private TMP_Text _soundsHeader;
    [SerializeField] private Slider _soundsSlider;
    [SerializeField] private TMP_Text _soundsValue;
    [SerializeField] private TMP_Text _graphicHeader;
    [SerializeField] private Slider _graphicSlider;
    [SerializeField] private TMP_Text _graphicValue;
    [SerializeField] private TMP_Text _languageHeader;
    [SerializeField] private TMP_Dropdown _languageDropdown;
    [SerializeField] private List<LanguageData> _languageData;
    
    private readonly GraphicQuality[] _qualities = (GraphicQuality[])Enum.GetValues(typeof(GraphicQuality));

    private List<Languages.Language> _languages;
    private int _currentQuality;

    private void Awake()
    {
        float musicVolume = SettingsStorage.MusicVolume.Value;
        UpdateMusicVolume(musicVolume);
        _musicSlider.value = musicVolume;
        _musicSlider.onValueChanged.AddListener(UpdateMusicVolume);
        
        float soundVolume = SettingsStorage.SoundVolume.Value;
        UpdateSoundsVolume(soundVolume);
        _soundsSlider.value = soundVolume;
        _soundsSlider.onValueChanged.AddListener(UpdateSoundsVolume);
        
        _graphicSlider.wholeNumbers = true;
        _graphicSlider.minValue = 0;
        _graphicSlider.maxValue = _qualities.Length - 1;
        int graphicValue = SettingsStorage.Graphic.Value;
        UpdateGraphic(graphicValue);
        _graphicSlider.value = graphicValue;
        _graphicSlider.onValueChanged.AddListener(UpdateGraphic);

        _languages = new List<Languages.Language>();
        List<TMP_Dropdown.OptionData> languagesOptions = new();
        for (var i = 0; i < _languageData.Count; i++)
        {
            LanguageData data = _languageData[i];
            languagesOptions.Add(new TMP_Dropdown.OptionData(data.LanguageName, data.Flag));
            _languages.Add(data.Language);
        }
        
        _languageDropdown.AddOptions(languagesOptions);
        _languageDropdown.onValueChanged.AddListener((index) =>
        {
            Localization.Instance.ChangeLanguage(_languages[index]);
        });
    }

    private void OnEnable()
    {
        Localization.Instance.OnLanguageUpdated += LocalizeText;
    }

    private void OnDisable()
    {
        Localization.Instance.OnLanguageUpdated -= LocalizeText;
    }

    private void Start()
    {
        LocalizeText();
    }

    private void LocalizeText()
    {
        _musicHeader.text = Localization.Instance[AllTexts.Music];
        _soundsHeader.text = Localization.Instance[AllTexts.Sounds];
        _graphicHeader.text = Localization.Instance[AllTexts.Graphic];
        _languageHeader.text = Localization.Instance[AllTexts.Language];
        _graphicValue.text = Localization.Instance[AllTexts.GraphicQualities[_qualities[_currentQuality]]];
    }

    private void UpdateMusicVolume(float newVolume)
    {
        _musicValue.text = ((int)(newVolume * 100)).ToString();
        SettingsStorage.MusicVolume.Value = newVolume;
        Music.Instance.ChangeMusicVolume(newVolume);
    }

    private void UpdateSoundsVolume(float newVolume)
    {
        _soundsValue.text = ((int)(newVolume * 100)).ToString();
        SettingsStorage.SoundVolume.Value = newVolume;
        Sounds.Instance.ChangeSoundsVolume(newVolume);
    }

    private void UpdateGraphic(float newValue)
    {
        _currentQuality = (int)newValue;
        _graphicValue.text = AllTexts.GraphicQualities[_qualities[_currentQuality]];
        SettingsStorage.Graphic.Value = _currentQuality;
        QualitySettings.SetQualityLevel(_currentQuality);
    }
}