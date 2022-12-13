using StarterAssets;
using UnityEngine;

public class FlyGravity : IGravitable
{
    private readonly float _gravity;
    private readonly float _flyHeight;
    
    private float _verticalVelocity;

    private MoneySpawner _moneySpawner;
    private ThirdPersonController _player;

    public FlyGravity(float gravity, float flyHeight, float speed, ThirdPersonController player, RunProgress runProgress)
    {
        _gravity = gravity;
        _flyHeight = flyHeight;
        _player = player;

        _moneySpawner = new MoneySpawner(
            new MoneyFactory<Money>(runProgress, false, true),
            flyHeight,
            gravity,
            speed,
            player.Controller.height);
    }
    
    public void EnterGravity()
    {
        _verticalVelocity = Mathf.Sqrt(_flyHeight * -2f * _gravity);
        _moneySpawner.SpawnMoneys(_player.gameObject.transform.position);
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