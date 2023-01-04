using StarterAssets;
using UnityEngine;

public class DefaultGravity : IGravitable, IRollable
{
    private const float RollGravityMultiplayer = 3;
    
    private readonly float _fallTimeout;
    private readonly float _jumpTimeout;
    private readonly float _terminalVelocity;
    private readonly float _gravity;

    private readonly ThirdPersonController _player;
    private readonly MovingInput _movingInput;
    
    private readonly bool _hasAnimator;
    private readonly Animator _animator;
    private readonly int _animIDJump;
    private readonly int _animIDFreeFall;
    private readonly Roll _roll;
    
    private float _verticalVelocity;
    private float _fallTimeoutDelta;
    private float _jumpTimeoutDelta;

    public DefaultGravity(
        float fallTimeout,
        float jumpTimeout,
        float terminalVelocity,
        float gravity,
        MovingInput movingInput,
        ThirdPersonController player,
        Animator animator = null,
        int animIDJump = 0,
        int animIDFreeFall = 0,
        int animIDRoll = 0)
    {
        _fallTimeout = fallTimeout;
        _jumpTimeout = jumpTimeout;
        _terminalVelocity = terminalVelocity;
        _gravity = gravity;
        _movingInput = movingInput;
        _player = player;
        
        _jumpTimeoutDelta = _jumpTimeout;
        _fallTimeoutDelta = _fallTimeout;
        
        if (animator == null) return;
        _animator = animator;
        _hasAnimator = true;
        _animIDJump = animIDJump;
        _animIDFreeFall = animIDFreeFall;

        _roll = new Roll(player, movingInput, gravity * RollGravityMultiplayer, animator, animIDRoll, animIDJump);
    }

    public float VerticalVelocity(bool isGrounded)
    {
        if (isGrounded)
        {
            _fallTimeoutDelta = _fallTimeout;

            if (_hasAnimator)
            {
                _animator.SetBool(_animIDJump, false);
                _animator.SetBool(_animIDFreeFall, false);
            }

            if (_verticalVelocity < 0.0f)
            {
                _verticalVelocity = -2f;
            }

            if (_movingInput.IsJumpPressed && _jumpTimeoutDelta <= 0.0f)
            {
                _verticalVelocity = Mathf.Sqrt(_player.JumpHeight * -2f * _gravity);

                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, true);
                }
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
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDFreeFall, true);
                }
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
