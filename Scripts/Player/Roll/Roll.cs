using StarterAssets;
using UnityEngine;

public class Roll
{
    private readonly ThirdPersonController _player;
    private readonly Animator _animator;
    private readonly MovingInput _movingInput;
    private readonly int _animIDRoll;
    private readonly int _animIDJump;
    private readonly float _gravity;
    private readonly bool _isAnimatorNotNull;
    
    private bool _roll;
    private bool _rollEnd;

    public Roll(
        ThirdPersonController player,
        MovingInput movingInput,
        float gravity,
        Animator animator = null,
        int animIDRoll = 0,
        int animIDJump = 0)
    {
        _player = player;
        _movingInput = movingInput;
        _gravity = gravity;

        _rollEnd = true;
        
        if (animator == null) return;
        _animator = animator;
        _animIDRoll = animIDRoll;
        _animIDJump = animIDJump;
        _isAnimatorNotNull = _animator != null;
    }

    public float RollVelocity(bool isGrounded)
    {
        float verticalVelocity = 0;
        if (!isGrounded && _roll)
        {
            verticalVelocity += _gravity * Time.fixedDeltaTime;
        }
        else if (_roll)
        {
            _roll = false;
        }
            
        if (!_rollEnd)
        {
            return verticalVelocity;
        }
        
        if (_movingInput.IsRollPressed)
        {
            if (_isAnimatorNotNull)
            {
                _animator.SetBool(_animIDJump, false);
                _animator.Play(_animIDRoll);
            }

            _roll = true;
            _rollEnd = false;
            _player.Controller.height *= 0.25f;
            _player.Controller.center *= 0.25f;
        }

        return verticalVelocity;
    }

    public void EndRoll()
    {
        if (_rollEnd) return;
        
        _player.Controller.height *= 4f;
        _player.Controller.center *= 4f;
        _rollEnd = true;
        _roll = false;
    }
}