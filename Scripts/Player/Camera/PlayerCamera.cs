using Cinemachine;
using UnityEngine;

public class PlayerCamera : IRunnable
{
    private readonly CinemachineVirtualCamera _runCamera;
    private readonly CinemachineVirtualCamera _idleCamera;
    public PlayerCamera(CinemachineVirtualCamera runCamera, CinemachineVirtualCamera idleCamera)
    {
        _runCamera = runCamera;
        _idleCamera = idleCamera;
        SetIdleCamera();
    }

    public void StartRun()
    {
        SetRunCamera();
    }

    public void EndRun()
    {
        SetIdleCamera();
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