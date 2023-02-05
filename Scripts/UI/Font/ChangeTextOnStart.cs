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

    private TMP_Text _text;

    public abstract void StartRun();

    public abstract void EndRun();

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    protected async void ChangeTextDilate(float startValue, int dir)
    {
        float currentTime = 0;
        CancellationToken token = GlobalCancellationToken.Instance.CancellationToken;
        while (currentTime < ShowTimeMilliseconds)
        {
            _text.fontMaterial.SetFloat(
                ShaderUtilities.ID_FaceDilate,
                startValue + currentTime / ShowTimeMilliseconds * dir);
            
            await Task.Delay(DeltaTimeMilliseconds, token).ContinueWith(GlobalCancellationToken.EmptyTask);
            if (token.IsCancellationRequested)
            {
                _text.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, startValue + dir);
                return;
            }
            
            currentTime += DeltaTimeMilliseconds;
        }
        
        _text.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, startValue + dir);
    }
}