using System;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using Object = UnityEngine.Object;

public class PlayerFlyAnimator : PlayerBaseAnimator
{
    private const string FlyVisualResourceName = "FlyVisual";

    private FlyVisual _flyVisual;
    
    protected override string AnimatorResourceName => "FlyAnimator";

    public override void EnterAnimator()
    {
        base.EnterAnimator();
        
        _flyVisual.ChangeActiveState(true);
    }

    public override void ExitAnimator()
    {
        base.ExitAnimator();
        
        _flyVisual.ChangeActiveState(false);
    }

    public PlayerFlyAnimator(Animator playerAnimator, ThirdPersonController player) : base(playerAnimator)
    {
        var flyVisual = Resources.Load(FlyVisualResourceName) as GameObject;

        if (flyVisual == null)
        {
            throw new Exception($"Resources does not contain {FlyVisualResourceName} prefab");
        }

        flyVisual = Object.Instantiate(flyVisual);
        _flyVisual = flyVisual.GetComponent<FlyVisual>();
        _flyVisual.Init(player);
        _flyVisual.ChangeActiveState(false);
        
        _boolAnimations = new Dictionary<AnimationType, IBoolAnimation>
        {
            { AnimationType.Land, new EmptyBoolAnimation() },
            { AnimationType.Jump, new FlyTakeoffAnimation(_flyVisual)}
        };

        _floatAnimations = new Dictionary<AnimationType, IFloatAnimation>
        {
            { AnimationType.Run, new EmptyFloatAnimation() },
            { AnimationType.Speed, new FlyMoveAnimation(_flyVisual) },
            { AnimationType.HorizontalRun, new FlyHorizontalMoveAnimation(_flyVisual) }
        };
    }
}