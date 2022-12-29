using StarterAssets;
using UnityEngine;

public sealed class FlyGravity : IGravitable
{
    private readonly float _gravity;
    private readonly float _flyHeight;
    private readonly MoneySpawner _moneySpawner;
    private readonly ThirdPersonController _player;
    private readonly Map _map;
    
    private float _verticalVelocity;
    private float _length;

    public FlyGravity(
        float gravity,
        float flyHeight,
        float speed,
        ThirdPersonController player,
        MoneyFactory<Item> moneyFactory,
        Map map)
    {
        _gravity = gravity;
        _flyHeight = flyHeight;
        _player = player;
        _map = map;

        _moneySpawner = new MoneySpawner(
            moneyFactory,
            flyHeight,
            gravity,
            speed,
            player.Controller.height);
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
    }

    public void LeaveGravity()
    {
        _player.ChangeHorizontalMoveRestriction(_player.HorizontalMoveRestrictions[typeof(HorizontalMoveRestriction)]);
    }

    public float VerticalVelocity(bool isGrounded)
    {
        if (_verticalVelocity > 0)
        {
            _verticalVelocity += _gravity * Time.fixedDeltaTime;

            if (_verticalVelocity < 0)
                _verticalVelocity = 0;
        }
        
        return _verticalVelocity;
    }
}