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

    public FlyGravity(
        float gravity,
        float flyHeight,
        float speed,
        ThirdPersonController player,
        MoneyFactory<Item> moneyFactory,
        Map map,
        RunProgress runProgress,
        PlayerAnimator playerAnimator)
    {
        _gravity = gravity;
        _flyHeight = flyHeight;
        _player = player;
        _map = map;
        _playerAnimator = playerAnimator;

        _moneySpawner = new MoneySpawner(
            moneyFactory,
            flyHeight,
            gravity,
            speed,
            player.Controller.height,
            runProgress);
    }

    public void SetGravityLength(float length)
    {
        _length = length;
    }
    
    public void EnterGravity()
    {
        _verticalVelocity = Mathf.Sqrt(_flyHeight * -2f * _gravity);
        float endGravityPositionZ = _moneySpawner.SpawnMoneys(_player.gameObject.transform.position, _length);
        _map.Level.HideBlocksBeforePositionZ(endGravityPositionZ);
        _player.ChangeHorizontalMoveRestriction(_player.HorizontalMoveRestrictions[typeof(FlyHorizontalRestriction)]);
        
        _playerAnimator.ChangeAnimator(typeof(PlayerFlyAnimator));
    }

    public void LeaveGravity()
    {
        _player.ChangeHorizontalMoveRestriction(_player.HorizontalMoveRestrictions[typeof(HorizontalMoveRestriction)]);
        
        _playerAnimator.ChangeAnimator(typeof(PlayerDefaultAnimator));
    }

    public float VerticalVelocity(bool isGrounded)
    {
        if (_verticalVelocity > 0)
        {
            _playerAnimator.ChangeAnimationBool(AnimationType.Jump, true);
            
            _verticalVelocity += _gravity * Time.fixedDeltaTime;

            if (_verticalVelocity < 0)
                _verticalVelocity = 0;
        }
        else
        {
            _playerAnimator.ChangeAnimationBool(AnimationType.Jump, false);
        }
        
        return _verticalVelocity;
    }
}