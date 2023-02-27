using System.Collections.Generic;
using UnityEngine;

public class FollowerDefaultAnimator : PlayerBaseAnimator
{
    protected override string AnimatorResourceName => "FollowerDefaultAnimator";
    
    public FollowerDefaultAnimator(Animator followerAnimator, Transform follower, float rotationSmoothTime, int speedID) 
        : base(followerAnimator)
    {
        _floatAnimations = new Dictionary<AnimationType, IFloatAnimation>
        {
            { AnimationType.Speed, new FloatAnimation(followerAnimator, speedID)},
            { AnimationType.HorizontalRun, new DefaultHorizontalMoveAnimation(follower, rotationSmoothTime) },
        };
    }
}