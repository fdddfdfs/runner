using StarterAssets;
using UnityEngine;

public sealed class DefaultHorizontalMoveAnimation : IFloatAnimation
{
    private readonly ThirdPersonController _player;
    private float _rotationVelocity;
    
    public DefaultHorizontalMoveAnimation(ThirdPersonController player)
    {
        _player = player;
    }
    
    public void SetFloat(float value)
    {
        float targetRotation = Mathf.Atan2(value, 1) * Mathf.Rad2Deg;
        float rotation = Mathf.SmoothDampAngle(
            _player.transform.eulerAngles.y,
            targetRotation,
            ref _rotationVelocity,
            _player.RotationSmoothTime);
        
        _player.transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
    }
}