using UnityEngine;

public class PlayerIdleAnimator : PlayerBaseAnimator
{
    protected override string AnimatorResourceName => "IdleAnimator";

    public PlayerIdleAnimator(Animator playerAnimator) : base(playerAnimator) { }
}