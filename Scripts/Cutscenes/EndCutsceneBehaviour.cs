using System;
using UnityEngine;
using UnityEngine.Animations;

public class EndCutsceneBehaviour : StateMachineBehaviour
{
    public Cutscene Cutscene { get; set; }

    private bool _isEnter;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
        AnimatorControllerPlayable controller)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex, controller);

        _isEnter = true;
    }

    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateMove(animator, stateInfo, layerIndex);

        if (!_isEnter) return;

        if (stateInfo.normalizedTime >= 1)
        {
            Cutscene.EndCutscene();
            _isEnter = false;
        }
    }
}