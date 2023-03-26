using UnityEngine;
using UnityEngine.UI;

public sealed class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _menu;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _backToMenuButton;
    [SerializeField] private Run _run;
    [SerializeField] private PauseController _pauseController;

    public void ChangeMenuActive(bool isActive)
    {
        _menu.SetActive(isActive);

        if (isActive)
        {
            _restartButton.Select();
            Sounds.Instance.PlaySound(2, "Pause");
            
            Achievements.Instance.GetAchievement("Pause_1");
            Achievements.Instance.GetAchievement("Pause_2");
            Achievements.Instance.GetAchievement("Pause_3");
        }
    }

    private void Awake()
    {
        _restartButton.onClick.AddListener(() =>
        {
            _pauseController.ChangePauseState();
            
            _run.ApplyLoseResults();
            _run.EndRun();
            _run.StartRun();
            
            Achievements.Instance.GetAchievement("Pause");
        });
        
        _backToMenuButton.onClick.AddListener(() =>
        {
            _pauseController.ChangePauseState();
            
            _run.ApplyLoseResults();
            _run.BackToMenu(false);
        });
    }
}