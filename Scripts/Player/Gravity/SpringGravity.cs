using StarterAssets;
using UnityEngine;

public sealed class SpringGravity : IGravitable, IRollable
{
    private readonly ThirdPersonController _player;
    private readonly float _springGravity;
    private readonly float _springHeight;
    private readonly Roll _roll;
    private readonly MoneySpawner _moneySpawner;
    private readonly Map _map;

    private float _verticalVelocity;

    public SpringGravity(
        float springGravity,
        float springHeight,
        float speed,
        ThirdPersonController player,
        MovingInput movingInput,
        MoneyFactory<Item> moneyFactory,
        Map map,
        RunProgress runProgress,
        PlayerAnimator playerAnimator)
    {
        _springGravity = springGravity;
        _springHeight = springHeight;
        _player = player;
        _map = map;

        _roll = new Roll(player, movingInput, springGravity * 10, playerAnimator);
        _moneySpawner = new MoneySpawner(
            moneyFactory,
            springHeight,
            springGravity,
            speed,
            player.Controller.height,
            runProgress,
            new float[] { -Map.ColumnOffset, 0, Map.ColumnOffset });
    }

    public void EnterGravity()
    {
        _verticalVelocity = Mathf.Sqrt(_springHeight * -2f * _springGravity);
        float endGravityPositionZ = _moneySpawner.SpawnMoneys(
            _springHeight * 0.7f,
            _player.transform.position);
        _map.Level.HideBlocksBeforePositionZ(endGravityPositionZ);
        _player.ChangeHorizontalMoveRestriction(_player.HorizontalMoveRestrictions[typeof(FlyHorizontalRestriction)]);
    }
    
    public float VerticalVelocity(bool isGrounded)
    {
        if (isGrounded && _verticalVelocity < 0)
        {
            _player.ChangeGravitable(_player.Gravitables[typeof(DefaultGravity)]);
        }
        
        _verticalVelocity += _springGravity * Time.fixedDeltaTime;

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
        _player.ChangeHorizontalMoveRestriction(_player.HorizontalMoveRestrictions[typeof(HorizontalMoveRestriction)]);
    }
}