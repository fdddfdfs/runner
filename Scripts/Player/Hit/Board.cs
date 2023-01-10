using System;
using System.Collections;
using StarterAssets;
using UnityEngine;

public sealed class Board : IHittable
{
    private const float ActiveTime = 10f;
    private const float RecoverTime = 3f;
    
    private readonly WaitForSeconds _activateWaiter = new(ActiveTime);
    private readonly WaitForSeconds _recoveryWaiter = new(RecoverTime);
    private readonly ThirdPersonController _player;
    private readonly Map _map;
    private readonly ActiveItemsUI _activeItemsUI;
    private readonly PlayerAnimator _playerAnimator;
    
    private Coroutine _activeRoutine;
    private Coroutine _recoveryRoutine;

    private bool _isActive;
    private bool _isRecovery;

    public Board(ThirdPersonController player, Map map, ActiveItemsUI activeItemsUI, PlayerAnimator playerAnimator)
    {
        _player = player;
        _map = map;
        _activeItemsUI = activeItemsUI;
        _playerAnimator = playerAnimator;
    }
    
    public void Activate()
    {
        if (_isActive)
        {
            Coroutines.StopRoutine(_activeRoutine);
            _isActive = false;
        }
        
        _activeItemsUI.ShowNewItemEffect(ItemType.Board, ActiveTime);
        _activeRoutine = Coroutines.StartRoutine(Activated());
        _player.ChangeHittable(this);
        _playerAnimator.ChangeAnimator(typeof(PlayerBoardAnimator));
    }
    
    public bool Hit(HitType hitType)
    {
        switch (hitType)
        {
            case HitType.Soft when _isRecovery && _isActive:
                Coroutines.StopRoutine(_recoveryRoutine);
                Coroutines.StopRoutine(_activeRoutine);
                _isRecovery = false;
                _isActive = false;
                _map.Level.HideCurrentEnteredBlock();
                Deactivate();
                break;
            case HitType.Soft:
                _recoveryRoutine = Coroutines.StartRoutine(Recover());
                break;
            case HitType.Hard when _isActive:
                Coroutines.StopRoutine(_activeRoutine);
                _isActive = false;
                _map.Level.HideCurrentEnteredBlock();
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
        _player.ChangeHittable(_player.Hittables[typeof(PlayerHittable)]);
        _activeItemsUI.HideEffect(ItemType.Board);

        if (_isRecovery)
        {
            Coroutines.StopRoutine(_recoveryRoutine);
            _isRecovery = false;
        }
        
        _playerAnimator.ChangeAnimator(typeof(PlayerDefaultAnimator));
    }

    private IEnumerator Activated()
    {
        _isActive = true;
        
        yield return _activateWaiter;
        
        Deactivate();
        _isActive = false;
    }

    private IEnumerator Recover()
    {
        _isRecovery = true;
        
        yield return _recoveryWaiter;

        _isRecovery = false;
    }
}
