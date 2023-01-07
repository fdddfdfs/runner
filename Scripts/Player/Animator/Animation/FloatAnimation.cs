using UnityEngine;

public class FloatAnimation : IFloatAnimation
{
    private readonly Animator _animator;
    private readonly int _floatID;
    
    public FloatAnimation(Animator animator, int floatID)
    {
        _animator = animator;
        _floatID = floatID;
    }

    public void SetFloat(float value)
    {
        _animator.SetFloat(_floatID, value);
    }
}