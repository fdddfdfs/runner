using UnityEngine;

public sealed class DieBehaviour : StateMachineBehaviour
{
    private const float FadeMultiplier = 5;
    
    private LoseDecideMenu _loseDecideMenu;
    private Fade _fade;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        
        _fade.FadeIn(() =>
        {
            _loseDecideMenu.BackToMainMenu();
            _fade.FadeOut(null, FadeMultiplier);
        }, FadeMultiplier);
    }

    private void Awake()
    {
        _fade = FindObjectOfType<Fade>();
        _loseDecideMenu = FindObjectOfType<LoseDecideMenu>();
    }
}