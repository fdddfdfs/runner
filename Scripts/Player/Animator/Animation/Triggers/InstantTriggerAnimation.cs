using UnityEngine;

public sealed class InstantTriggerAnimation : ITriggerAnimation
{
    private readonly Animator _animator;
    private readonly int _animationID;

    public InstantTriggerAnimation(Animator animator, int animationID)
    {
        _animator = animator;
        _animationID = animationID;
    }

    public void SetTrigger()
    {
        _animator.Play(_animationID);
    }
}