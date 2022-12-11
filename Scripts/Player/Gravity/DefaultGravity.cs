using StarterAssets;
using UnityEngine;

public class DefaultGravity : IGravitable
{
    private readonly float _fallTimeout;
    private readonly float _jumpTimeout;
    private readonly float _terminalVelocity;
    private readonly float _gravity;
    private readonly float _jumpHeight;
    
    private readonly StarterAssetsInputs _input;
    
    private readonly bool _hasAnimator;
    private readonly Animator _animator;
    private readonly int _animIDJump;
    private readonly int _animIDFreeFall;
    
    private float _verticalVelocity;
    private float _fallTimeoutDelta;
    private float _jumpTimeoutDelta;

    public DefaultGravity(
        float fallTimeout,
        float jumpTimeout,
        float terminalVelocity,
        float gravity,
        float jumpHeight,
        StarterAssetsInputs input,
        Animator animator = null,
        int animIDJump = 0,
        int animIDFreeFall = 0)
    {
        _fallTimeout = fallTimeout;
        _jumpTimeout = jumpTimeout;
        _jumpHeight = jumpHeight;
        _terminalVelocity = terminalVelocity;
        _gravity = gravity;
        _input = input;
        
        if (animator == null) return;
        _animator = animator;
        _hasAnimator = true;
        _animIDJump = animIDJump;
        _animIDFreeFall = animIDFreeFall;

        _jumpTimeoutDelta = _jumpTimeout;
        _fallTimeoutDelta = _fallTimeout;
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

            if (_input.jump && _jumpTimeoutDelta <= 0.0f)
            {
                _verticalVelocity = Mathf.Sqrt(_jumpHeight * -2f * _gravity);

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

            _input.jump = false;
        }

        if (_verticalVelocity < _terminalVelocity)
        {
            _verticalVelocity += _gravity * Time.deltaTime;
        }

        return _verticalVelocity;
    }
}
