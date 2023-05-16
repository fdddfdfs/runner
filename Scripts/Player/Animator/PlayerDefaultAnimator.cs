using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public sealed class PlayerDefaultAnimator : PlayerBaseAnimator
{
    protected override string AnimatorResourceName => "DefaultAnimator";

    private readonly DefaultHorizontalMoveAnimation _defaultHorizontalMoveAnimation;

    public PlayerDefaultAnimator(
        Animator playerAnimator,
        IReadOnlyDictionary<AnimationType, int> animationsID,
        ThirdPersonController player)
        : base(playerAnimator)
    {
        _triggerAnimations = new Dictionary<AnimationType, ITriggerAnimation>
        {
            { AnimationType.Roll, new InstantTriggerAnimation(playerAnimator, animationsID[AnimationType.Roll]) },
            { AnimationType.Die, new TriggerAnimation(playerAnimator, animationsID[AnimationType.Die])},
            { AnimationType.DieRight, new TriggerAnimation(playerAnimator, animationsID[AnimationType.DieRight])},
            { AnimationType.DieLeft, new TriggerAnimation(playerAnimator, animationsID[AnimationType.DieLeft])},
            { AnimationType.Resurrect, new TriggerAnimation(playerAnimator, animationsID[AnimationType.Resurrect])},
            { AnimationType.SoftHitLeft, new TriggerAnimation(playerAnimator, animationsID[AnimationType.SoftHitLeft])},
            { AnimationType.SoftHitRight, new TriggerAnimation(playerAnimator, animationsID[AnimationType.SoftHitRight])},
            { AnimationType.Lose, new TriggerAnimation(playerAnimator, animationsID[AnimationType.Lose])},
            { AnimationType.Reset, new TriggerAnimation(playerAnimator, animationsID[AnimationType.Reset])}
        };

        _boolAnimations = new Dictionary<AnimationType, IBoolAnimation>
        {
            { AnimationType.Jump, new BoolAnimation(playerAnimator, animationsID[AnimationType.Jump]) },
            { AnimationType.Land, new BoolAnimation(playerAnimator, animationsID[AnimationType.Land]) },
            { AnimationType.Fall, new BoolAnimation(playerAnimator, animationsID[AnimationType.Fall]) },
        };

        _defaultHorizontalMoveAnimation = new DefaultHorizontalMoveAnimation(
            player.transform,
            player.RotationSmoothTime);
        
        _floatAnimations = new Dictionary<AnimationType, IFloatAnimation>
        {
            { AnimationType.Speed, new FloatAnimation(playerAnimator, animationsID[AnimationType.Speed]) },
            { AnimationType.HitSpeed, new FloatAnimation(playerAnimator, animationsID[AnimationType.HitSpeed]) },
            { AnimationType.HorizontalRun, _defaultHorizontalMoveAnimation }
        };
    }

    public override void ExitAnimator()
    {
        base.ExitAnimator();
        
        _defaultHorizontalMoveAnimation.ForceSetFloat(0);
    }
}