using TMPro;
using UnityEngine;

public class NicknameLocalization : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    private string _defaultText;
    
    private void OnEnable()
    {
        _text.text = SteamManager.Initialized ? 
            Steamworks.SteamFriends.GetPersonaName() :
            Localization.Instance[_defaultText];
    }

    private void Awake()
    {
        _defaultText = _text.text;
    }
}