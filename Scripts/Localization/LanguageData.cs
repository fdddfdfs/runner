using UnityEngine;

[CreateAssetMenu(fileName = "Language", menuName = "Languages/Language")]
public class LanguageData : ScriptableObject
{
    [SerializeField] private string _languageName;
    [SerializeField] private Languages.Language _language;
    [SerializeField] private Sprite _flag;

    public string LanguageName => _languageName;
    public Languages.Language Language => _language;
    public Sprite Flag => _flag;
}