using System;
using UnityEngine;

public class FadeInBehaviour : StateMachineBehaviour
{
    public Fade Fade { get; set; }

    private bool _isEnter;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        _isEnter = true;
    }

    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateMove(animator, stateInfo, layerIndex);

        if (!_isEnter) return;
        
        if (stateInfo.normalizedTime >= 1)
        {
            Fade.FinishFadeIn();
            _isEnter = false;
        }
    }
}