﻿using System;
using UnityEngine;

public class EnvironmentAnimatedObject : MonoBehaviour, ITriggerable
{
    private const string StartAnimationTriggerName = "Start";
    
    [SerializeField] private Animator _objectAnimator;

    private readonly int _startAnimationTrigger = Animator.StringToHash(StartAnimationTriggerName);

    private void Awake()
    {
        if (_objectAnimator == null)
        {
            _objectAnimator = GetComponent<Animator>();

            if (_objectAnimator == null)
            {
                throw new Exception("Cannot find animator");
            }
        }
    }

    public void Trigger()
    {
        _objectAnimator.SetTrigger(_startAnimationTrigger);
    }
}