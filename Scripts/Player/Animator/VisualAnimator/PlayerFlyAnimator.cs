using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public sealed class PlayerFlyAnimator : PlayerVisualAnimator
{
    protected override string AnimatorResourceName => "FlyAnimator";

    protected override string VisualResourceName => "FlyVisual";

    public PlayerFlyAnimator(Animator playerAnimator, ThirdPersonController player) : base(playerAnimator, player)
    {
        _triggerAnimations = new Dictionary<AnimationType, ITriggerAnimation>();
        
        _boolAnimations = new Dictionary<AnimationType, IBoolAnimation>
        {
            { AnimationType.Land, new EmptyBoolAnimation() },
            { AnimationType.Jump, new VisualTakeoffAnimation(_visual)}
        };

        _floatAnimations = new Dictionary<AnimationType, IFloatAnimation>
        {
            { AnimationType.Speed, new VisualMoveAnimation(_visual) },
            { AnimationType.HorizontalRun, new VisualZMoveAnimation(_visual) }
        };
    }
}