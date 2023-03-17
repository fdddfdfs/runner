using System;
using System.Threading;
using StarterAssets;
using UnityEngine;

public sealed class Board : IHittable
{
    private const float ActiveTime = 10f;
    private const float RecoverTime = 3f;
    private const float EffectYOffset = 1;
    private const int EffectTimeMilliseconds = 5000;
    
    private readonly ThirdPersonController _player;
    private readonly Map _map;
    private readonly ICancellationTokenProvider _cancellationTokenProvider;
    private readonly CancellationToken[] _linkedTokens;
    private readonly Effects _effects;
    
    private CancellationTokenSource _cancellationTokenSource;

    private bool _isActive;
    private bool _isRecovery;

    public Board(
        ThirdPersonController player,
        Map map,
        ICancellationTokenProvider cancellationTokenProvider,
        Effects effects)
    {
        _player = player;
        _map = map;
        _cancellationTokenProvider = cancellationTokenProvider;
        _linkedTokens = new[] { cancellationTokenProvider.GetCancellationToken() };
        _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(_linkedTokens);
        _effects = effects;
    }
    
    public void Activate()
    {
        if (_isActive)
        {
            _cancellationTokenSource.Cancel();
            RecreateCancellationToken();
            
            Deactivate();
            _isActive = false;
        }
        else if(_cancellationTokenSource.IsCancellationRequested)
        {
            RecreateCancellationToken();
        }
        
        Activated();
        _player.ChangeHittable(this);
    }

    public bool Hit(HitType hitType)
    {
        switch (hitType)
        {
            case HitType.Soft when _isRecovery && _isActive:
                _cancellationTokenSource.Cancel();
                _isRecovery = false;
                _isActive = false;
                _map.Level.HideCurrentEnteredBlock();
                _effects.ActivateEffect(
                    EffectType.Explosion,
                    _player.transform.position + EffectYOffset * Vector3.up,
                    EffectTimeMilliseconds);
                Deactivate();
                break;
            case HitType.Soft:
                Recover();
                break;
            case HitType.Hard when _isActive:
                _cancellationTokenSource.Cancel();
                _isActive = false;
                _map.Level.HideCurrentEnteredBlock();
                _effects.ActivateEffect(
                    EffectType.Explosion,
                    _player.transform.position + EffectYOffset * Vector3.up,
                    EffectTimeMilliseconds);
                Deactivate();
                break;
            case HitType.Hard:
                return true;
            default:
                throw new ArgumentOutOfRangeException(nameof(hitType), hitType, null);
        }

        return false;
    }

    private void Deactivate()
    {
        _player.PlayerStateMachine.ChangeStateSafely(typeof(BoardState), typeof(RunState));

        if (_isRecovery)
        {
            _cancellationTokenSource.Cancel();
            _isRecovery = false;
        }
    }
    
    private async void Activated()
    {
        _isActive = true;

        await AsyncUtils.Wait(ActiveTime, _cancellationTokenSource.Token);

        if (_cancellationTokenSource.IsCancellationRequested) return;
        
        Deactivate();
        _isActive = false;
    }

    private async void Recover()
    {
        _isRecovery = true;

        await AsyncUtils.Wait(RecoverTime, _cancellationTokenSource.Token);

        _isRecovery = false;
    }
    
    private void RecreateCancellationToken()
    {
        _cancellationTokenSource.Dispose();
        _linkedTokens[0] = _cancellationTokenProvider.GetCancellationToken();
        _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(_linkedTokens);
    }
}
