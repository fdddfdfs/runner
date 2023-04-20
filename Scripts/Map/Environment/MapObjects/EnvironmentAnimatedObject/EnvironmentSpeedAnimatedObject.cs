using System;
using UnityEngine;

public class EnvironmentSpeedAnimatedObject : EnvironmentAnimatedObject
{
    private const string SpeedParameterName = "Speed";

    private readonly int _speedParameter = Animator.StringToHash(SpeedParameterName);

    private RunProgress _runProgress;
    
    public override void Trigger()
    {
        base.Trigger();
        
        _objectAnimator.SetFloat(_speedParameter, _runProgress.SpeedMultiplier);
    }

    private void Start()
    {
        _runProgress = FindObjectOfType<RunProgress>();
    }
}