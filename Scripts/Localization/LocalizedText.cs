using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LocalizedText : MonoBehaviour
{
    [SerializeField] private List<TMP_Text> _textsObjects;

    private List<(string baseTextLocalization, TMP_Text textObject)> _initialTexts;
    
    private void Awake()
    {
        _initialTexts = new List<(string baseTextLocalization, TMP_Text textObject)>();

        foreach (TMP_Text textsObject in _textsObjects)
        {
            _initialTexts.Add((textsObject.text, textsObject));
        }
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
        Dictionary<string, string> localization = Localization.Instance.LocalizationDictionary;
        
        foreach ((string baseTextLocalization, TMP_Text textObject) initialText in _initialTexts)
        {
            initialText.textObject.text = localization[initialText.baseTextLocalization];
        }
    }
}