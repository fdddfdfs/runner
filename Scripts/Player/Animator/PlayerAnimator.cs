using System;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public sealed class PlayerAnimator : BaseAnimator
{
    private readonly ThirdPersonController _player;

    public Type CurrentAnimatorType { get; private set; }

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
        
        ChangeAnimator(typeof(PlayerDefaultAnimator));
    }

    public override void ChangeAnimator(Type animatorType)
    {
        if (CurrentAnimatorType == animatorType) return;
        
        CurrentAnimatorType = animatorType;
        
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
            { AnimationType.HitSpeed, Animator.StringToHash("HitSpeed") },
            { AnimationType.Roll, Animator.StringToHash("Roll") },
            { AnimationType.Die, Animator.StringToHash("Die")},
            { AnimationType.DieRight, Animator.StringToHash("DieRight")},
            { AnimationType.DieLeft, Animator.StringToHash("DieLeft")},
            { AnimationType.Resurrect, Animator.StringToHash("Resurrect")},
            { AnimationType.SoftHitLeft, Animator.StringToHash("SoftHitLeft")},
            { AnimationType.SoftHitRight, Animator.StringToHash("SoftHitRight")},
            { AnimationType.Lose, Animator.StringToHash("Lose")},
            { AnimationType.Reset, Animator.StringToHash("Reset")},
        };
    }
}