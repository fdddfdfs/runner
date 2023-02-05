using System;
using System.Threading.Tasks;
using StarterAssets;
using UnityEngine;

public sealed class PlayerHittable : IHittable
{
    private const float RecoverTime = 20f;
    private const int RecoverTimeMilliseconds = (int)(RecoverTime * 1000);
    
    private readonly ThirdPersonController _player;
    private readonly Follower _follower;
    private readonly Run _run;
    
    private Coroutine _recoveryRoutine;
    private bool _isRecovery;

    public PlayerHittable(ThirdPersonController player, Follower follower, Run run)
    {
        _player = player;
        _follower = follower;
        _run = run;
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
                Recover();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(hitType), hitType, "Unknown hit type");
        }

        return false;
    }
    
    private async void Recover()
    {
        _isRecovery = true;
        _follower.FollowForTime(_player.gameObject, RecoverTime);

        await Task.Delay(RecoverTimeMilliseconds, _run.EndRunToken);

        _isRecovery = false;
    }
}
