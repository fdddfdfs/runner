using UnityEngine;
using UnityEngine.UI;

public sealed class PauseMenu : MonoBehaviour
{
    private const float FadeMultiplier = 5f;
    
    [SerializeField] private GameObject _menu;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _backToMenuButton;
    [SerializeField] private Run _run;
    [SerializeField] private PauseController _pauseController;
    [SerializeField] private Fade _fade;

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
            _fade.FadeIn(() =>
            {
                _pauseController.ChangePauseState();
            
                _run.ApplyRunResults();
                _run.EndRun();
                _run.StartRun();
                
                _fade.FadeOut(null, FadeMultiplier);
            }, FadeMultiplier);

            Achievements.Instance.GetAchievement("Restart");
        });
        
        _backToMenuButton.onClick.AddListener(() =>
        {
            _pauseController.ChangePauseState();
            
            _run.ApplyRunResults();
            _run.BackToMenu(false);
        });
    }
}