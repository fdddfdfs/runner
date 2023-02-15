using UnityEngine;

public class FadeOutBehaviour : StateMachineBehaviour
{
    public Fade Fade { get; set; }

    private bool _isEntered;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        _isEntered = true;
    }
    
    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateMove(animator, stateInfo, layerIndex);

        if (!_isEntered) return;
        
        if (stateInfo.normalizedTime >= 1)
        {
            _isEntered = false;
            Fade.FinishFadeOut();
        }
    }
}