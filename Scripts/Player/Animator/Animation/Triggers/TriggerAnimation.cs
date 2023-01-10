using UnityEngine;

public sealed class TriggerAnimation : ITriggerAnimation
{
    private readonly Animator _animator;
    private readonly int _triggerID;
    
    public TriggerAnimation(Animator animator, int triggerID)
    {
        _animator = animator;
        _triggerID = triggerID;
    }

    public void SetTrigger()
    {
        _animator.SetTrigger(_triggerID);
    }
}