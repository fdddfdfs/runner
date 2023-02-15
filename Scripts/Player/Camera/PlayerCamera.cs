using Cinemachine;
using UnityEngine;

public class PlayerCamera : IRunnable
{
    private readonly CinemachineVirtualCamera _runCamera;
    private readonly CinemachineVirtualCamera _idleCamera;
    private readonly Camera _camera;
    
    public PlayerCamera(Camera camera, CinemachineVirtualCamera runCamera, CinemachineVirtualCamera idleCamera)
    {
        _runCamera = runCamera;
        _idleCamera = idleCamera;
        _camera = camera;
        _idleCamera.gameObject.SetActive(false);
        _runCamera.gameObject.SetActive(false);
        _camera.gameObject.SetActive(false);
    }

    public void StartRun()
    {
        _camera.gameObject.SetActive(true);
        _runCamera.gameObject.SetActive(true);
    }

    public void EndRun()
    {
        _runCamera.gameObject.SetActive(false);
        _camera.gameObject.SetActive(false);
    }

    private void SetIdleCamera()
    {
        _idleCamera.gameObject.SetActive(true);
        _runCamera.gameObject.SetActive(false);
    }

    private void SetRunCamera()
    {
        _idleCamera.gameObject.SetActive(false);
        _runCamera.gameObject.SetActive(true);
    }
}