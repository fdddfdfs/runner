﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class FollowerAnimator : BaseAnimator
{
    private const float RotationSmoothTime = 0.12f;
    
    public FollowerAnimator(Animator animator, Transform follower)
    {
        _animators = new Dictionary<Type, PlayerBaseAnimator>
        {
            { typeof(FollowerDefaultAnimator), new FollowerDefaultAnimator(animator, follower, RotationSmoothTime) },
        };

        ChangeAnimator(typeof(FollowerDefaultAnimator));
    }
}