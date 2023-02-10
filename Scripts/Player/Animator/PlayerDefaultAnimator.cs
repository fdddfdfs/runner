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
            { AnimationType.HorizontalRun, _defaultHorizontalMoveAnimation }
        };
    }

    public override void ExitAnimator()
    {
        base.ExitAnimator();
        
        _defaultHorizontalMoveAnimation.ForceSetFloat(0);
    }
}