using StarterAssets;
using UnityEngine;

public sealed class DefaultGravity : IGravitable, IRollable
{
    private const float RollGravityMultiplayer = 3;
    
    private readonly float _fallTimeout;
    private readonly float _jumpTimeout;
    private readonly float _terminalVelocity;
    private readonly float _gravity;

    private readonly ThirdPersonController _player;
    private readonly PlayerRunInput _playerRunInput;

    private readonly PlayerAnimator _playerAnimator;
    private readonly Roll _roll;
    
    private float _verticalVelocity;
    private float _fallTimeoutDelta;
    private float _jumpTimeoutDelta;

    public DefaultGravity(
        float fallTimeout,
        float jumpTimeout,
        float terminalVelocity,
        float gravity,
        PlayerRunInput playerRunInput,
        ThirdPersonController player, 
        PlayerAnimator playerAnimator)
    {
        _fallTimeout = fallTimeout;
        _jumpTimeout = jumpTimeout;
        _terminalVelocity = terminalVelocity;
        _gravity = gravity;
        _playerRunInput = playerRunInput;
        _player = player;
        
        _jumpTimeoutDelta = _jumpTimeout;
        _fallTimeoutDelta = _fallTimeout;

        _playerAnimator = playerAnimator;

        _roll = new Roll(player, playerRunInput, gravity * RollGravityMultiplayer, playerAnimator);
    }

    public float VerticalVelocity(bool isGrounded)
    {
        if (isGrounded)
        {
            _fallTimeoutDelta = _fallTimeout;

            _playerAnimator.ChangeAnimationBool(AnimationType.Jump, false);
            _playerAnimator.ChangeAnimationBool(AnimationType.Fall, false);

            if (_verticalVelocity < 0.0f)
            {
                _verticalVelocity = -2f;
            }

            if (_playerRunInput.IsJumpPressed && _jumpTimeoutDelta <= 0.0f)
            {
                _verticalVelocity = Mathf.Sqrt(_player.JumpHeight * -2f * _gravity);

                _playerAnimator.ChangeAnimationBool(AnimationType.Jump, true);
            }

            if (_jumpTimeoutDelta >= 0.0f)
            {
                _jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else
        {
            _jumpTimeoutDelta = _jumpTimeout;

            if (_fallTimeoutDelta >= 0.0f)
            {
                _fallTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                _playerAnimator.ChangeAnimationBool(AnimationType.Fall, true);
            }
        }

        if (_verticalVelocity < _terminalVelocity)
        {
            _verticalVelocity += _gravity * Time.deltaTime;
        }

        return _verticalVelocity;
    }

    public void Roll(bool isGrounded)
    {
        _verticalVelocity += _roll.RollVelocity(isGrounded);
    }

    public void EndRoll()
    {
        _roll.EndRoll();
    }

    public void LeaveGravity()
    {
        EndRoll();
        _verticalVelocity = 0;
    }
}
