using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ExitMenu : Menu
{
    [SerializeField] private Button _yesButton;
    [SerializeField] private Button _noButton;

    public void SwapMenuActive()
    {
        _menu.SetActive(!_menu.activeSelf);

        if (_menu.activeSelf)
        {
            Sounds.Instance.PlayRandomSounds(2,"Exit");
        }
    }
    
    private void Awake()
    {
        _yesButton.onClick.AddListener(Application.Quit);
        _noButton.onClick.AddListener(() => _menu.SetActive(false));
    }
}