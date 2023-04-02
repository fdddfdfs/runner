using System;
using System.Threading;
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
    private CancellationToken[] _linkedTokens;
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
        _linkedTokens = new[] { AsyncUtils.Instance.GetCancellationToken() };
        _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(_linkedTokens);
    }

    protected async void ChangeTextDilate(float startValue, int dir)
    {
        CheckCancellationToken();
        
        Text.gameObject.SetActive(true);
        
        _isActive = true;
        float currentTime = 0;
        CancellationToken token = _cancellationTokenSource.Token;
        
        while (currentTime < ShowTimeMilliseconds)
        {
            Text.fontMaterial.SetFloat(
                ShaderUtilities.ID_FaceDilate,
                startValue + currentTime / ShowTimeMilliseconds * dir);

            await AsyncUtils.Wait(DeltaTimeMilliseconds, token, true);
            if (token.IsCancellationRequested)
            {
                Text.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, startValue + dir);

                return;
            }

            currentTime += DeltaTimeMilliseconds;
        }

        _isActive = false;
        Text.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, startValue + dir);

        if (Math.Abs(startValue + dir) > 0.9f)
        {
            Text.gameObject.SetActive(false);
        }
    }

    private void CheckCancellationToken()
    {
        if (_isActive || _cancellationTokenSource.IsCancellationRequested)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _linkedTokens[0] = AsyncUtils.Instance.GetCancellationToken();
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(_linkedTokens);
        }
    }
}