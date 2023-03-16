using UnityEngine;
using UnityEngine.Animations;

public sealed class EndCutsceneBehaviour : StateMachineBehaviour
{
    private Cutscene _cutscene;
    private float _endThreshold;
    private bool _isEnter;

    public void Init(Cutscene cutscene, float endThreshold = 1)
    {
        _cutscene = cutscene;
        _endThreshold = endThreshold;
    }

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

        if (stateInfo.normalizedTime >= _endThreshold)
        {
            _cutscene.EndCutscene();
            _isEnter = false;
        }
    }
}