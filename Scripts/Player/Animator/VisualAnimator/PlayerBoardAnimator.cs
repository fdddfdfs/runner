using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public sealed class PlayerBoardAnimator : PlayerVisualAnimator
{
    protected override string AnimatorResourceName => "BoardAnimator";

    protected override string VisualResourceName => "BoardVisual";

    public PlayerBoardAnimator(
        Animator playerAnimator,
        IReadOnlyDictionary<AnimationType, int> animationsID,
        ThirdPersonController player) 
        : base(playerAnimator, player)
    {
        _triggerAnimations = new Dictionary<AnimationType, ITriggerAnimation>
        {
            { AnimationType.Roll, new InstantTriggerAnimation(playerAnimator, animationsID[AnimationType.Roll]) },
        };

        _boolAnimations = new Dictionary<AnimationType, IBoolAnimation>
        {
            { AnimationType.Land, new EmptyBoolAnimation() },
            { AnimationType.Jump, new EmptyBoolAnimation() },
            { AnimationType.Fall, new EmptyBoolAnimation() },
        };

        _floatAnimations = new Dictionary<AnimationType, IFloatAnimation>
        {
            { AnimationType.Speed, new VisualMoveAnimation(_visual) },
            { AnimationType.HorizontalRun, new VisualYMoveAnimation(_visual) }
        };
    }
}