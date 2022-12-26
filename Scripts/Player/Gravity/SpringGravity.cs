using StarterAssets;
using UnityEngine;

public sealed class SpringGravity : IGravitable, IRollable
{
    private readonly ThirdPersonController _player;
    private readonly float _springGravity;
    private readonly float _springHeight;
    private readonly Roll _roll;
    private readonly MoneySpawner _moneySpawner;

    private float _verticalVelocity;

    public SpringGravity(
        float springGravity,
        float springHeight,
        float speed,
        ThirdPersonController player,
        MovingInput movingInput,
        MoneyFactory<Item> moneyFactory,
        Animator animator = null,
        int animIDRoll = 0,
        int animIDJump = 0)
    {
        _springGravity = springGravity;
        _springHeight = springHeight;
        _player = player;

        _roll = new Roll(player, movingInput, springGravity * 10, animator, animIDRoll, animIDJump);
        _moneySpawner = new MoneySpawner(
            moneyFactory,
            springHeight,
            springGravity,
            speed,
            player.Controller.height,
            new float[] { -Level.ColumnOffset, 0, Level.ColumnOffset });
    }

    public void EnterGravity()
    {
        _verticalVelocity = Mathf.Sqrt(_springHeight * -2f * _springGravity);
        _moneySpawner.SpawnMoneys(_springHeight * 0.7f, _player.transform.position);
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
    }
}