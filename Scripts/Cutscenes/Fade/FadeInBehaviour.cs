using UnityEngine;

public class FadeInBehaviour : StateMachineBehaviour
{
    public Fade Fade { get; set; }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        
        Fade.FinishFadeIn();
    }
}