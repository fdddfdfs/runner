using System;
using System.Collections;
using StarterAssets;
using UnityEngine;

public sealed class PlayerHittable : IHittable
{
    private const float RecoverTime = 20f;
    
    private readonly WaitForSeconds _recoveryWaiter = new(RecoverTime);
    private readonly ThirdPersonController _player;
    private readonly Follower _follower;
    
    private Coroutine _recoveryRoutine;
    private bool _isRecovery;

    public PlayerHittable(ThirdPersonController player, Follower follower)
    {
        _player = player;
        _follower = follower;
    }
    
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
        _follower.FollowForTime(_player.gameObject, RecoverTime);
        
        yield return _recoveryWaiter;

        _isRecovery = false;
    }
}
