using StarterAssets;
using UnityEngine;
using Transform = UnityEngine.Transform;

public sealed class DefaultHorizontalMoveAnimation : IFloatAnimation
{
    private readonly Transform _player;
    private readonly float _smoothTime;
    
    private float _rotationVelocity;
    
    public DefaultHorizontalMoveAnimation(Transform player, float smoothTime)
    {
        _player = player;
        _smoothTime = smoothTime;
    }
    
    public void SetFloat(float value)
    {
        float targetRotation = Mathf.Atan2(value, 1) * Mathf.Rad2Deg;
        float rotation = Mathf.SmoothDampAngle(
            _player.transform.eulerAngles.y,
            targetRotation,
            ref _rotationVelocity,
            _smoothTime);
        
        _player.transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
    }

    public void ForceSetFloat(float value)
    {
        _player.transform.rotation = Quaternion.Euler(0.0f, value, 0.0f);
    }
}