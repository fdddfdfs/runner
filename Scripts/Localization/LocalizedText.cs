using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LocalizedText : MonoBehaviour
{
    [SerializeField] protected List<TMP_Text> _textsObjects;

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

    protected virtual void LocalizeText()
    {
        foreach ((string baseTextLocalization, TMP_Text textObject) initialText in _initialTexts)
        {
            initialText.textObject.text = Localization.Instance[initialText.baseTextLocalization];
        }
    }
}