using System;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public sealed class PlayerAnimator
{
    private readonly Dictionary<Type, PlayerBaseAnimator> _animators;
    private readonly ThirdPersonController _player;

    private PlayerBaseAnimator _current;
    
    public PlayerAnimator(Animator playerAnimator, ThirdPersonController player)
    {
        _player = player;
        
        Dictionary<AnimationType, int> animationsID = GetAnimationIDs();

        _animators = new Dictionary<Type, PlayerBaseAnimator>
        {
            { typeof(PlayerDefaultAnimator), new PlayerDefaultAnimator(playerAnimator, animationsID, player) },
            { typeof(PlayerFlyAnimator), new PlayerFlyAnimator(playerAnimator, player) },
            { typeof(PlayerBoardAnimator), new PlayerBoardAnimator(playerAnimator, animationsID, player) },
            { typeof(PlayerImmuneAnimator), new PlayerImmuneAnimator(playerAnimator, animationsID, player) },
        };
        
        ChangeAnimator(typeof(PlayerDefaultAnimator));
    }

    public void ChangeAnimator(Type animatorType)
    {
        _current?.ExitAnimator();
        
        _current = _animators[animatorType];
        
        _current.EnterAnimator();
        if (_player.IsRoll)
        {
            _current.ChangeAnimationTrigger(AnimationType.Roll);
        }
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

    private static Dictionary<AnimationType, int> GetAnimationIDs()
    {
        return new Dictionary<AnimationType, int>
        {
            { AnimationType.Land, Animator.StringToHash("Grounded") },
            { AnimationType.Jump, Animator.StringToHash("Jump") },
            { AnimationType.Fall, Animator.StringToHash("FreeFall") },
            { AnimationType.Speed, Animator.StringToHash("Speed") },
            { AnimationType.Roll, Animator.StringToHash("Roll") },
        };
    }
}