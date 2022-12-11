using System;
using System.Collections;
using UnityEngine;

public class PlayerHittable : IHittable
{
    private const float RecoverTime = 3f;
    
    private readonly WaitForSeconds _recoveryWaiter = new(RecoverTime);
    
    private Coroutine _recoveryRoutine;
    private bool _isRecovery;
    
    public bool Hit(HitType hitType)
    {
        switch (hitType)
        {
            case HitType.Hard:
                return true;
            case HitType.Soft when _isRecovery:
                Coroutines.StopRoutine(_recoveryRoutine);
                _isRecovery = false;
                return true;
            case HitType.Soft:
                _recoveryRoutine = Coroutines.StartRoutine(Recover());
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(hitType), hitType, null);
        }

        return false;
    }
    
    private IEnumerator Recover()
    {
        _isRecovery = true;
        
        yield return _recoveryWaiter;

        _isRecovery = false;
    }
}
