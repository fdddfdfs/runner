using UnityEngine;

public sealed class RollBehaviour : StateMachineBehaviour
{
    private StarterAssets.ThirdPersonController _thirdPersonController;
    private void Awake()
    {
        _thirdPersonController = FindObjectOfType<StarterAssets.ThirdPersonController>();
    }
    
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _thirdPersonController.EndRoll();
    }
}
