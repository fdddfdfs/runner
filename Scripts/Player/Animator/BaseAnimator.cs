using System;
using System.Collections.Generic;

public abstract class BaseAnimator
{
    protected Dictionary<Type, PlayerBaseAnimator> _animators;
    
    protected PlayerBaseAnimator _current;

    public virtual void ChangeAnimator(Type animatorType)
    {
        _current?.ExitAnimator();

        _current = _animators[animatorType];

        _current.EnterAnimator();
    }
    
    public void ChangeAnimationTrigger(AnimationType animation)
    {
        _current.ChangeAnimationTrigger(animation);
    }

    public void ChangeAnimationBool(AnimationType animation, bool value)
    {
        _current.ChangeAnimationBool(animation, value);
    }

    public void ChangeAnimationFloat(AnimationType animation, float value)
    {
        _current.ChangeAnimationFloat(animation, value);
    }
}