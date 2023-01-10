using StarterAssets;
using UnityEngine;

public sealed class Roll
{
    private readonly ThirdPersonController _player;
    private readonly PlayerAnimator _playerAnimator;
    private readonly PlayerRunInput _playerRunInput;
    private readonly float _gravity;

    private bool _roll;
    private bool _rollEnd;

    public Roll(ThirdPersonController player, PlayerRunInput playerRunInput, float gravity, PlayerAnimator playerAnimator)
    {
        _player = player;
        _playerRunInput = playerRunInput;
        _gravity = gravity;
        _playerAnimator = playerAnimator;
        _rollEnd = true;
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
        
        if (_playerRunInput.IsRollPressed)
        {
            _playerAnimator.ChangeAnimationBool(AnimationType.Jump, false);
            _playerAnimator.ChangeAnimationTrigger(AnimationType.Roll);

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