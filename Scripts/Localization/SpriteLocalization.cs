using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpriteLocalization : LocalizedText
{
    [SerializeField] private LocalizedSprite _localizedSprite;
    [SerializeField] private Image _image;
    [SerializeField] private Sprite _defaultSprite;
    
    protected override void LocalizeText()
    {
        if ((Languages.Language)SettingsStorage.Localization.Value == _localizedSprite.Language)
        {
            _image.sprite = _localizedSprite.Sprite;

            foreach (TMP_Text text in _textsObjects)
            {
                text.text = string.Empty;
            }
        }
        else
        {
            base.LocalizeText();
            _image.sprite = _defaultSprite;
        }
    }

    [Serializable]
    private struct LocalizedSprite
    {
        public Languages.Language Language;

        public Sprite Sprite;
    }
}