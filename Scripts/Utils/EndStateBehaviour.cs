using System;
using UnityEngine;

public class EndStateBehaviour : StateMachineBehaviour
{
    private Action _onEndState;
    private float _endThreshold;

    private bool _isEnter;
    
    public void Init(Action onEndState, float endThreshold = 1)
    {
        _onEndState = onEndState;
        _endThreshold = endThreshold;
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        _isEnter = true;
    }

    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateMove(animator, stateInfo, layerIndex);

        if (!_isEnter) return;
        
        if (stateInfo.normalizedTime >= _endThreshold)
        {
            _onEndState?.Invoke();
            _isEnter = false;
        }
    }
}