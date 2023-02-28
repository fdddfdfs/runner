using System;
using System.Threading;
using System.Threading.Tasks;
using StarterAssets;
using UnityEngine;

public sealed class PlayerHittable : IHittable
{
    private const float RecoverTime = 20f;
    private const int RecoverTimeMilliseconds = (int)(RecoverTime * 1000);
    
    private readonly ThirdPersonController _player;
    private readonly Follower _follower;
    private readonly ICancellationTokenProvider _cancellationTokenProvider;

    private CancellationTokenSource _recoverCancellationSource;
    private bool _isRecovery;

    public PlayerHittable(
        ThirdPersonController player,
        Follower follower,
        ICancellationTokenProvider cancellationTokenProvider)
    {
        _player = player;
        _follower = follower;
        _cancellationTokenProvider = cancellationTokenProvider;
        
        _recoverCancellationSource = 
            CancellationTokenSource.CreateLinkedTokenSource(_cancellationTokenProvider.GetCancellationToken());
    }
    
    public bool Hit(HitType hitType)
    {
        switch (hitType)
        {
            case HitType.Hard:
                return true;
            case HitType.Soft when _isRecovery:
                _recoverCancellationSource.Cancel();
                return true;
            case HitType.Soft:
                Recover();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(hitType), hitType, "Unknown hit type");
        }

        return false;
    }

    public void StopRecover()
    {
        if (_isRecovery)
        {
            _recoverCancellationSource.Cancel();
        }
    }
    
    private async void Recover()
    {
        _isRecovery = true;
        _follower.FollowForTime(_player.gameObject, RecoverTime);

        try
        {
            await Task.Delay(RecoverTimeMilliseconds, _recoverCancellationSource.Token);
        }
        catch
        {
            _follower.StopFollowing();
            _recoverCancellationSource.Dispose();
            _recoverCancellationSource = 
                CancellationTokenSource.CreateLinkedTokenSource(_cancellationTokenProvider.GetCancellationToken());
        }

        _isRecovery = false;
    }
}
