using UnityEngine;

public class BoolAnimation : IBoolAnimation
{
    private readonly Animator _animator;
    private readonly int _boolID;
    
    public BoolAnimation(Animator animator, int triggerID)
    {
        _animator = animator;
        _boolID = triggerID;
    }

    public void SetBool(bool value)
    {
        _animator.SetBool(_boolID, value);
    }
}