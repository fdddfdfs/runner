using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseAnimator
{
    protected Dictionary<AnimationType, ITriggerAnimation> _triggerAnimations;
    protected Dictionary<AnimationType, IBoolAnimation> _boolAnimations;
    protected Dictionary<AnimationType, IFloatAnimation> _floatAnimations;
    
    protected abstract string AnimatorResourceName { get; }

    public RuntimeAnimatorController AnimatorController { get;}

    protected PlayerBaseAnimator()
    {
        if (AnimatorResourceName == null)
        {
            throw new Exception("AnimatorResourceName get method need to be initialized");
        }
        
        AnimatorController = Resources.Load(AnimatorResourceName) as RuntimeAnimatorController;

        if (AnimatorController == null)
        {
            throw new Exception($"Resources does not contain {AnimatorResourceName} animation controller");
        }
    }

    public void ChangeAnimationTrigger(AnimationType animation)
    {
        if (!_triggerAnimations.ContainsKey(animation))
        {
            throw new Exception($"Trigger do not exist for {animation} in {this}");
        }
        
        _triggerAnimations[animation].SetTrigger();
    }

    public void ChangeAnimationBool(AnimationType animation, bool value)
    {
        if (!_boolAnimations.ContainsKey(animation))
        {
            throw new Exception($"Bool do not exist for {animation} in {this}");
        }
        
        _boolAnimations[animation].SetBool(value);
    }

    public void ChangeAnimationFloat(AnimationType animation, float value)
    {
        if (!_floatAnimations.ContainsKey(animation))
        {
            throw new Exception($"Float do not exist for {animation} in {this}");
        }
        
        _floatAnimations[animation].SetFloat(value);
    }
}