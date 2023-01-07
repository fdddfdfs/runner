using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator
{
    private readonly Dictionary<Type, PlayerBaseAnimator> _animators;
    
    private readonly Animator _playerAnimator;
    private PlayerBaseAnimator _current;
    
    public PlayerAnimator(Animator playerAnimator)
    {
        _playerAnimator = playerAnimator;

        Dictionary<AnimationType, int> animationsID = new Dictionary<AnimationType, int>();

        _animators = new Dictionary<Type, PlayerBaseAnimator>
        {
            { typeof(PlayerDefaultAnimator), new PlayerDefaultAnimator(playerAnimator, animationsID) }
        };
    }

    public void ChangeAnimator(Type animatorType)
    {
        _current = _animators[animatorType];
        _playerAnimator.runtimeAnimatorController = _current.AnimatorController;
    }

    public void ChangeAnimationTrigger(AnimationType animation)
    {
        _current.ChangeAnimationTrigger(animation);
    }

    public void ChangeAnimationBool(AnimationType animation, bool value)
    {
        _current.ChangeAnimationBool(animation, value);
    }

    public void ChangeAnimationFloat(AnimationType animation, float value)
    {
        _current.ChangeAnimationFloat(animation, value);
    }
}