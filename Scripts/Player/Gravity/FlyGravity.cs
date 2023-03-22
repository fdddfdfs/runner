using StarterAssets;
using UnityEngine;

public sealed class FlyGravity : IGravitable
{
    private readonly float _gravity;
    private readonly float _flyHeight;
    private readonly MoneySpawner _moneySpawner;
    private readonly ThirdPersonController _player;
    private readonly Map _map;
    private readonly PlayerAnimator _playerAnimator;
    
    private float _verticalVelocity;
    private float _length;
    private float _currentTime;

    public FlyGravity(
        float gravity,
        float flyHeight,
        float speed,
        ThirdPersonController player,
        Map map,
        RunProgress runProgress,
        Run run,
        PlayerAnimator playerAnimator)
    {
        _gravity = gravity;
        _flyHeight = flyHeight;
        _player = player;
        _map = map;
        _playerAnimator = playerAnimator;

        _moneySpawner = new MoneySpawner(
            flyHeight,
            gravity,
            speed,
            player.Controller.height,
            runProgress,
            run,
            player.Effects);
    }

    public void SetGravityLength(float length)
    {
        _length = length;
    }
    
    public void EnterGravity()
    {
        _verticalVelocity = Mathf.Sqrt(_flyHeight * -2f * _gravity);
        float endGravityPositionZ = _moneySpawner.SpawnMoneys(_player.gameObject.transform.position, _length);
        //_map.Level.HideBlocksBeforePositionZ(endGravityPositionZ);
    }

    public void LeaveGravity()
    {
        _currentTime = 0;
    }

    public float VerticalVelocity(bool isGrounded)
    {
        _currentTime += Time.deltaTime;
        
        if (_verticalVelocity > 0)
        {
            _playerAnimator.ChangeAnimationBool(AnimationType.Jump, true);
            
            _verticalVelocity += _gravity * Time.fixedDeltaTime;

            if (_verticalVelocity < 0)
                _verticalVelocity = 0;
        }
        else if(_currentTime < _length)
        {
            _playerAnimator.ChangeAnimationBool(AnimationType.Jump, false);
        }
        else if(!isGrounded)
        {
            _playerAnimator.ChangeAnimationBool(AnimationType.Fall, true);
            _verticalVelocity += _gravity * Time.fixedDeltaTime;
        }
        else
        {
            _player.PlayerStateMachine.ChangeState(typeof(RunState));
        }

        return _verticalVelocity;
    }
}