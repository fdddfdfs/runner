using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class FollowerAnimator : BaseAnimator
{
    private const float RotationSmoothTime = 0.12f;
    
    public FollowerAnimator(Animator animator, Transform follower)
    {
        int speedAnimationID = Animator.StringToHash("Speed");
        int loseAnimationID = Animator.StringToHash("Lose");
        int borderLoseAnimationID = Animator.StringToHash("LoseBorder");
        
        _animators = new Dictionary<Type, PlayerBaseAnimator>
        {
            { typeof(FollowerDefaultAnimator), 
                new FollowerDefaultAnimator(
                    animator,
                    follower,
                    RotationSmoothTime,
                    speedAnimationID,
                    loseAnimationID,
                    borderLoseAnimationID) },
        };

        ChangeAnimator(typeof(FollowerDefaultAnimator));
    }
}