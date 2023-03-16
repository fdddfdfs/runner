using System.Collections.Generic;
using UnityEngine;

public sealed class FollowerDefaultAnimator : PlayerBaseAnimator
{
    protected override string AnimatorResourceName => "FollowerDefaultAnimator";
    
    public FollowerDefaultAnimator(
        Animator followerAnimator,
        Transform follower,
        float rotationSmoothTime,
        int speedID,
        int loseID) 
        : base(followerAnimator)
    {
        _triggerAnimations = new Dictionary<AnimationType, ITriggerAnimation>
        {
            [AnimationType.Lose] = new TriggerAnimation(followerAnimator, loseID),
        };
        
        _floatAnimations = new Dictionary<AnimationType, IFloatAnimation>
        {
            { AnimationType.Speed, new FloatAnimation(followerAnimator, speedID)},
            { AnimationType.HorizontalRun, new DefaultHorizontalMoveAnimation(follower, rotationSmoothTime) },
        };
    }
}