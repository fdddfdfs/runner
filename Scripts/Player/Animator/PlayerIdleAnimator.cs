using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleAnimator : PlayerBaseAnimator
{
    protected override string AnimatorResourceName => "IdleAnimator";

    public PlayerIdleAnimator(Animator playerAnimator) : base(playerAnimator)
    {
        _triggerAnimations = new Dictionary<AnimationType, ITriggerAnimation>
        {
            { AnimationType.Roll, new EmptyTriggerAnimation() },
        };

        _boolAnimations = new Dictionary<AnimationType, IBoolAnimation>
        {
            { AnimationType.Jump, new EmptyBoolAnimation() },
            { AnimationType.Land, new EmptyBoolAnimation() },
            { AnimationType.Fall, new EmptyBoolAnimation() },
        };
        
        _floatAnimations = new Dictionary<AnimationType, IFloatAnimation>
        {
            { AnimationType.Speed, new EmptyFloatAnimation() },
            { AnimationType.HorizontalRun, new EmptyFloatAnimation() }
        };
    }
}