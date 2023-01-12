using System;
using StarterAssets;
using UnityEngine;
using Object = UnityEngine.Object;

public abstract class PlayerVisualAnimator : PlayerBaseAnimator
{
    protected readonly Visual _visual;
    
    protected abstract string VisualResourceName { get; }
    
    protected PlayerVisualAnimator(Animator playerAnimator, ThirdPersonController player) : base(playerAnimator)
    {
        var visual = Resources.Load(VisualResourceName) as GameObject;

        if (visual == null)
        {
            throw new Exception($"Resources does not contain {VisualResourceName} prefab");
        }

        visual = Object.Instantiate(visual);
        _visual = visual.GetComponent<Visual>();
        _visual.Init(player);
        _visual.gameObject.SetActive(false);
    }
    
    public override void EnterAnimator()
    {
        base.EnterAnimator();
        
        _visual.ChangeActiveState(true);
    }

    public override void ExitAnimator()
    {
        base.ExitAnimator();
        
        _visual.ChangeActiveState(false);
    }
}