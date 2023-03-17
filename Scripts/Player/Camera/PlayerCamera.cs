using System.Threading;
using System.Threading.Tasks;
using Cinemachine;
using UnityEngine;

public class PlayerCamera : IRunnable
{
    private readonly CinemachineVirtualCamera _runCamera;
    private readonly CinemachineVirtualCamera _idleCamera;
    private readonly Camera _camera;
    private readonly ICancellationTokenProvider _cancellationTokenProvider;

    private readonly float _startAmplitude;
    private readonly CinemachineBasicMultiChannelPerlin _cinemachineBasicMultiChannelPerlin;
    
    public PlayerCamera(
        Camera camera,
        CinemachineVirtualCamera runCamera,
        CinemachineVirtualCamera idleCamera,
        ICancellationTokenProvider cancellationTokenProvider)
    {
        _runCamera = runCamera;
        _idleCamera = idleCamera;
        _camera = camera;
        _cancellationTokenProvider = cancellationTokenProvider;
        _cinemachineBasicMultiChannelPerlin = _runCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _startAmplitude = _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain;

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

    public async void ShakeCamera(float intensity, float time)
    {
        _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        float currentTime = 0;
        CancellationToken token = _cancellationTokenProvider.GetCancellationToken();
        while (currentTime < time)
        {
            await Task.Yield();
            currentTime += Time.unscaledDeltaTime * AsyncUtils.TimeScale;
            _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain =
                Mathf.Lerp(_startAmplitude, intensity, 1 - currentTime/time);

            if (token.IsCancellationRequested) break;
        }

        _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = _startAmplitude;
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