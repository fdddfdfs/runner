using System;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public sealed class PlayerAnimator : BaseAnimator
{
    private readonly ThirdPersonController _player;

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
            { typeof(PlayerIdleAnimator), new PlayerIdleAnimator(playerAnimator) },
        };
        
        ChangeAnimator(typeof(PlayerIdleAnimator));
    }

    public override void ChangeAnimator(Type animatorType)
    {
        base.ChangeAnimator(animatorType);
        
        if (_player.IsRoll)
        {
            _current.ChangeAnimationTrigger(AnimationType.Roll);
        }
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