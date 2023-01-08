using System.Runtime.CompilerServices;
using StarterAssets;
using UnityEngine;

public class FlyVisual : MonoBehaviour
{
    private const float TargetHorizontalRotation = 30;
    private const float TargetTakeoffRotation = 30;
    
    [SerializeField] private Transform _rotateObject;

    private ThirdPersonController _player;
    private float _zVelocity;
    private float _xVelocity;

    public void Init(ThirdPersonController player)
    {
        _player = player;
    }

    public void Move(float speed)
    {
        _rotateObject.localRotation *= Quaternion.Euler(0, 0, -1 * speed * Time.timeScale);
        transform.position = _player.transform.position;
    }

    public void HorizontalMove(float dir)
    {
        if (dir != 0)
        {
            dir = dir < 0 ? -1 : 1;
        }

        float rotation = Mathf.SmoothDampAngle(
            transform.eulerAngles.z,
            TargetHorizontalRotation * dir,
            ref _zVelocity,
            _player.RotationSmoothTime);

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 180, rotation);
    }

    public void Takeoff(bool state)
    {
        float dir = state ? 1 : 0;
        
        float rotation = Mathf.SmoothDampAngle(
            transform.eulerAngles.x,
            TargetTakeoffRotation * dir,
            ref _xVelocity,
            _player.RotationSmoothTime);

        transform.rotation = Quaternion.Euler(rotation, 180, transform.rotation.eulerAngles.z);
    }

    public void ChangeActiveState(bool state)
    {
        gameObject.SetActive(state);
    }
}