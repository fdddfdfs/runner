﻿using Palmmedia.ReportGenerator.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : Menu
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

    private readonly string[] _qualities = { "Very Low", "Low", "Medium", "High", "Very High", "Ultra" };

    private void Awake()
    {
        _musicHeader.text = "Music";
        float musicVolume = SettingsData.MusicVolume.Value;
        UpdateMusicVolume(musicVolume);
        _musicSlider.value = musicVolume;
        _musicSlider.onValueChanged.AddListener(UpdateMusicVolume);

        _soundsHeader.text = "Sounds";
        float soundVolume = SettingsData.SoundVolume.Value;
        UpdateSoundsVolume(soundVolume);
        _soundsSlider.value = soundVolume;
        _soundsSlider.onValueChanged.AddListener(UpdateSoundsVolume);

        _graphicHeader.text = "Graphic";
        _graphicSlider.wholeNumbers = true;
        _graphicSlider.minValue = 0;
        _graphicSlider.maxValue = _qualities.Length - 1;
        int graphicValue = SettingsData.Graphic.Value;
        UpdateGraphic(graphicValue);
        _graphicSlider.value = graphicValue;
        _graphicSlider.onValueChanged.AddListener(UpdateGraphic);
    }

    private void UpdateMusicVolume(float newVolume)
    {
        _musicValue.text = ((int)(newVolume * 100)).ToString();
        SettingsData.MusicVolume.Value = newVolume;
        Music.Instance.ChangeMusicVolume(newVolume);
    }

    private void UpdateSoundsVolume(float newVolume)
    {
        _soundsValue.text = ((int)(newVolume * 100)).ToString();
        SettingsData.SoundVolume.Value = newVolume;
        Sounds.Instance.ChangeSoundsVolume(newVolume);
    }

    private void UpdateGraphic(float newValue)
    {
        var newValueInt = (int)newValue;
        _graphicValue.text = _qualities[newValueInt];
        SettingsData.Graphic.Value = newValueInt;
        QualitySettings.SetQualityLevel(newValueInt);
    }
}