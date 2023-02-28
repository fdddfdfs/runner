using System;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public abstract class ChangeTextOnStart : MonoBehaviour, IRunnable
{
    private const float ShowTime = 1f;
    private const float ShowTimeMilliseconds = ShowTime * 1000;
    private const float ShowDelta = 0.1f;
    private const int DeltaTimeMilliseconds = (int)(ShowDelta / ShowTime * 1000);

    private CancellationTokenSource _cancellationTokenSource;
    private bool _isActive;

    public TMP_Text Text { get; private set; }

    public abstract void StartRun();

    public abstract void EndRun();

    public void SetDilate(float value)
    {
        Text.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, value);
    }

    private void Awake()
    {
        Text = GetComponent<TMP_Text>();
        _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(
            GlobalCancellationToken.Instance.GetCancellationToken());
    }

    protected async void ChangeTextDilate(float startValue, int dir)
    {
        CheckCancellationToken();
        
        _isActive = true;
        float currentTime = 0;
        while (currentTime < ShowTimeMilliseconds)
        {
            Text.fontMaterial.SetFloat(
                ShaderUtilities.ID_FaceDilate,
                startValue + currentTime / ShowTimeMilliseconds * dir);

            try
            {
                await Task.Delay(DeltaTimeMilliseconds, _cancellationTokenSource.Token);
            }
            catch
            {
                Text.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, startValue + dir);
                return;
            }

            currentTime += DeltaTimeMilliseconds;
        }

        _isActive = false;
        Text.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, startValue + dir);
    }

    private void CheckCancellationToken()
    {
        if (_isActive || _cancellationTokenSource.IsCancellationRequested)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(
                GlobalCancellationToken.Instance.GetCancellationToken());
        }
    }
}