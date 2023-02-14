using UnityEngine;

public class EndCutsceneBehaviour : StateMachineBehaviour
{
    public Cutscene Cutscene { get; set; }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        
        Cutscene.EndCutscene();
    }
}