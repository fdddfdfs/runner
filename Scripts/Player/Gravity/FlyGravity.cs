using UnityEngine;

public class FlyGravity : IGravitable
{
    private readonly float _gravity;
    private readonly float _flyHeight;
    
    private float _verticalVelocity;

    public FlyGravity(float gravity, float flyHeight)
    {
        _gravity = gravity;
        _flyHeight = flyHeight;
    }
    
    public void EnterGravity()
    {
        _verticalVelocity = Mathf.Sqrt(_flyHeight * -2f * _gravity);
    }

    public float VerticalVelocity(bool isGrounded)
    {
        if (_verticalVelocity > 0)
        {
            _verticalVelocity += _gravity * Time.deltaTime;

            if (_verticalVelocity < 0)
                _verticalVelocity = 0;
        }
        
        return _verticalVelocity;
    }
}