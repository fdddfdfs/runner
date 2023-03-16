using UnityEngine;

public sealed class DieBehaviour : StateMachineBehaviour
{
    private LoseDecideMenu _loseDecideMenu;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        
        _loseDecideMenu.BackToMainMenu();
    }
    
    private void Awake()
    {
        _loseDecideMenu = FindObjectOfType<LoseDecideMenu>();
    }
}