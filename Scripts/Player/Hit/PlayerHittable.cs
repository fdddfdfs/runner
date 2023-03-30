using System;
using System.Threading;
using System.Threading.Tasks;
using StarterAssets;
using UnityEngine;

public sealed class PlayerHittable : IHittable
{
    private const float RecoverTime = 20f;

    private readonly ThirdPersonController _player;
    private readonly Follower _follower;
    private readonly ICancellationTokenProvider _cancellationTokenProvider;
    private readonly CancellationToken[] _linkedTokens;

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
        _linkedTokens = new CancellationToken[1];
    }
    
    public bool Hit(HitType hitType)
    {
        switch (hitType)
        {
            case HitType.Hard:
                return true;
            case HitType.Soft when _isRecovery:
                _recoverCancellationSource.Cancel();
                Sounds.Instance.PlayRandomSounds(2, "Hit");
                return true;
            case HitType.Soft:
                Recover();
                Sounds.Instance.PlayRandomSounds(2, "Hit");
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
        
        if (_recoverCancellationSource == null || _recoverCancellationSource.IsCancellationRequested)
        {
            CreateCancellationToken();
        }

        await AsyncUtils.Wait(RecoverTime, _recoverCancellationSource.Token);
        
        if (_recoverCancellationSource.IsCancellationRequested)
        {
            _follower.StopFollowing();
            CreateCancellationToken();
        }
        else
        {
            Sounds.Instance.PlaySound(2, "StopRecover");
        }

        _isRecovery = false;
    }

    private void CreateCancellationToken()
    {
        _recoverCancellationSource?.Dispose();
        _linkedTokens[0] = _cancellationTokenProvider.GetCancellationToken();
        _recoverCancellationSource = CancellationTokenSource.CreateLinkedTokenSource(_linkedTokens);
    }
}
