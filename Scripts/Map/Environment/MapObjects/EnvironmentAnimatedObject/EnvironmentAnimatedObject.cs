using UnityEngine;

public class EnvironmentAnimatedObject : MonoBehaviour, ITriggerable
{
    private const string StartAnimationTriggerName = "Start";
    
    [SerializeField] private Animator _objectAnimator;

    private readonly int _startAnimationTrigger = Animator.StringToHash(StartAnimationTriggerName);
    
    public void Trigger()
    {
        _objectAnimator.SetTrigger(_startAnimationTrigger);
    }
}