using StarterAssets;
using UnityEngine;

public class SpringGravity : IGravitable, IRollable
{
    private readonly ThirdPersonController _player;
    private readonly float _springGravity;
    private readonly float _springHeight;
    private readonly float _speed;
    private readonly Roll _roll;

    private float _verticalVelocity;

    public SpringGravity(
        float springGravity,
        float springHeight,
        float speed,
        ThirdPersonController player,
        MovingInput movingInput,
        Animator animator = null,
        int animIDRoll = 0,
        int animIDJump = 0)
    {
        _springGravity = springGravity;
        _springHeight = springHeight;
        _speed = speed;
        _player = player;

        _roll = new Roll(player, movingInput, springGravity * 10, animator, animIDRoll, animIDJump);
    }

    public void EnterGravity()
    {
        _verticalVelocity = Mathf.Sqrt(_springHeight * -2f * _springGravity);
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