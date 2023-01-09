using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public abstract class Visual : MonoBehaviour
{
    private const float TargetXRotation = 30;
    private const float TargetYRotation = 30;
    private const float TargetZRotation = 30;
    private const float BaseSpeed = 2;
    
    [SerializeField] private List<Transform> _rotators;

    private ThirdPersonController _player;
    
    private float _xVelocity;
    private float _yVelocity;
    private float _zVelocity;
    
    protected abstract Quaternion RotatorAngle { get; }
    
    public void Init(ThirdPersonController player)
    {
        _player = player;
    }
    
    public void Move(float speed)
    {
        foreach (Transform rotator in _rotators)
        {
            rotator.localRotation *= Quaternion.Euler(RotatorAngle.eulerAngles * (BaseSpeed * speed * Time.timeScale));
        }
        
        transform.position = _player.transform.position;
    }
    
    public void ChangeActiveState(bool state)
    {
        gameObject.SetActive(state);
    }

    public void MoveX(float dir)
    {
        if (dir != 0)
        {
            dir = dir < 0 ? 1 : -1;
        }

        float rotation = Mathf.SmoothDampAngle(
            transform.eulerAngles.x,
            TargetXRotation * dir,
            ref _xVelocity,
            _player.RotationSmoothTime);

        var objectRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(
            rotation,
            objectRotation.y,
            objectRotation.z);

        var playerRotation = _player.PlayerMesh.localRotation.eulerAngles;
        _player.PlayerMesh.localRotation = Quaternion.Euler(
            rotation,
            playerRotation.y,
            playerRotation.z);
    }
    
    public void MoveY(float dir)
    {
        if (dir != 0)
        {
            dir = dir < 0 ? 1 : -1;
        }

        float rotation = Mathf.SmoothDampAngle(
            transform.eulerAngles.y,
            TargetYRotation * dir,
            ref _yVelocity,
            _player.RotationSmoothTime);

        var objectRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(
            objectRotation.x,
            rotation,
            objectRotation.z);

        var playerRotation = _player.PlayerMesh.localRotation.eulerAngles;
        _player.PlayerMesh.localRotation = Quaternion.Euler(
            playerRotation.x,
            rotation,
            playerRotation.z);
    }
    
    public void MoveZ(float dir)
    {
        if (dir != 0)
        {
            dir = dir < 0 ? 1 : -1;
        }

        float rotation = Mathf.SmoothDampAngle(
            transform.eulerAngles.z,
            TargetZRotation * dir,
            ref _zVelocity,
            _player.RotationSmoothTime);

        var objectRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(
            objectRotation.x,
            objectRotation.y,
            rotation);

        var playerRotation = _player.PlayerMesh.rotation.eulerAngles;
        _player.PlayerMesh.rotation = Quaternion.Euler(
            playerRotation.x,
            playerRotation.y,
            rotation);
    }
}