using System.Threading;
using System.Threading.Tasks;
using Cinemachine;
using UnityEngine;
using Quaternion = System.Numerics.Quaternion;

public sealed class PlayerCamera : IRunnable
{
    private readonly CinemachineVirtualCamera _runCamera;
    private readonly CinemachineVirtualCamera _idleCamera;
    private readonly Camera _camera;
    private readonly ICancellationTokenProvider _cancellationTokenProvider;
    private readonly CancellationToken[] _linkedTokens = new CancellationToken[1];

    private readonly float _startAmplitude;
    private readonly CinemachineBasicMultiChannelPerlin _cinemachineBasicMultiChannelPerlin;

    private CancellationTokenSource _cancellationTokenSource;

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
        _camera.transform.rotation = UnityEngine.Quaternion.Euler(0, 0, 0);
        _camera.gameObject.SetActive(true);
        _runCamera.gameObject.SetActive(true);
    }

    public void EndRun()
    {
        _runCamera.gameObject.SetActive(false);
        _camera.gameObject.SetActive(false);
        _camera.transform.position = Vector3.zero;
        _camera.transform.rotation = UnityEngine.Quaternion.Euler(0, 180, 0);
    }

    public async void ShakeCamera(float intensity, float time)
    {
        RecreateTokenSource();
        
        _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        float currentTime = 0;
        CancellationToken token = _cancellationTokenSource.Token;
        while (currentTime < time)
        {
            await Task.Yield();
            currentTime += Time.unscaledDeltaTime;
            _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain =
                Mathf.Lerp(_startAmplitude, intensity, 1 - currentTime/time);

            if (token.IsCancellationRequested) break;
        }

        _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = _startAmplitude;
    }

    public void StopShake()
    {
        _cancellationTokenSource?.Cancel();
    }

    private void RecreateTokenSource()
    {
        if (_cancellationTokenSource == null || _cancellationTokenSource.IsCancellationRequested)
        {
            _cancellationTokenSource?.Dispose();
            _linkedTokens[0] = _cancellationTokenProvider.GetCancellationToken();
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(_linkedTokens);
        }
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