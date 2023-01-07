using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerDefaultAnimator : PlayerBaseAnimator
{
    protected override string AnimatorResourceName => "DefaultAnimator";
    
    public PlayerDefaultAnimator(Animator playerAnimator, IReadOnlyDictionary<AnimationType, int> animationsID)
    {
        _triggerAnimations = new Dictionary<AnimationType, ITriggerAnimation>
        {
            { AnimationType.Roll, new InstantTriggerAnimation(playerAnimator, animationsID[AnimationType.Roll]) },
        };

        _boolAnimations = new Dictionary<AnimationType, IBoolAnimation>
        {
            { AnimationType.Jump, new BoolAnimation(playerAnimator, animationsID[AnimationType.Jump]) },
            { AnimationType.Land, new BoolAnimation(playerAnimator, animationsID[AnimationType.Land]) },
            { AnimationType.Fall, new BoolAnimation(playerAnimator, animationsID[AnimationType.Fall]) },
        };

        _floatAnimations = new Dictionary<AnimationType, IFloatAnimation>
        {
            { AnimationType.Run, new FloatAnimation(playerAnimator, animationsID[AnimationType.Run]) },
            { AnimationType.Speed, new FloatAnimation(playerAnimator, animationsID[AnimationType.Speed]) }
        };
    }
}