using System.Collections.Generic;
using UnityEngine;

public class FollowerDefaultAnimator : PlayerBaseAnimator
{
    protected override string AnimatorResourceName => "FollowerDefaultAnimator";
    
    public FollowerDefaultAnimator(Animator playerAnimator, Transform follower, float rotationSmoothTime) 
        : base(playerAnimator)
    {
        _floatAnimations = new Dictionary<AnimationType, IFloatAnimation>
        {
            { AnimationType.HorizontalRun, new DefaultHorizontalMoveAnimation(follower, rotationSmoothTime) },
        };
    }
}