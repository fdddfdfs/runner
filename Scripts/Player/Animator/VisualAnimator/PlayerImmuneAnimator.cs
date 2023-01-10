using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public sealed class PlayerImmuneAnimator : PlayerVisualAnimator
{
    protected override string AnimatorResourceName => "ImmuneAnimator";
    
    protected override string VisualResourceName => "ImmuneVisual";
    
    public PlayerImmuneAnimator(Animator playerAnimator, ThirdPersonController player) : base(playerAnimator, player)
    {
        //TODO: make roll animation for board 
        _triggerAnimations = new Dictionary<AnimationType, ITriggerAnimation>()
        {
            { AnimationType.Roll, new EmptyTriggerAnimation() },
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