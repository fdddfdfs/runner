using StarterAssets;
using UnityEngine;

public class DefaultGravity : IGravitable, IRollable
{
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
    private readonly int _animIDRoll;
    
    private float _verticalVelocity;
    private float _fallTimeoutDelta;
    private float _jumpTimeoutDelta;
    private bool _roll;
    private bool _rollEnd;

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
        _animIDRoll = animIDRoll;

        _rollEnd = true;
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
        if (!isGrounded && _roll)
        {
            _verticalVelocity += _gravity * Time.fixedDeltaTime * 3;
        }
        else if (_roll)
        {
            _roll = false;
        }
            
        if (!_rollEnd)
        {
            return;
        }
        
        if (_movingInput.IsRollPressed)
        {
            _animator.SetBool(_animIDJump, false);
            _animator.Play(_animIDRoll);
            _roll = true;
            _rollEnd = false;
            _player.Controller.height *= 0.25f;
            _player.Controller.center *= 0.25f;
        }
    }

    public void EndRoll()
    {
        _player.Controller.height *= 4f;
        _player.Controller.center *= 4f;
        _rollEnd = true;
        _roll = false;
    }

    public void LeaveGravity()
    {
        if (!_rollEnd)
        {
            EndRoll();
        }
    }
}
