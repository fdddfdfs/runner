using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class ShowFont : MonoBehaviour, IRunnable
{
    private const float ShowTime = 1f;
    private const float ShowTimeMilliseconds = ShowTime * 1000;
    private const float ShowDelta = 0.1f;
    private const int DeltaTimeMilliseconds = (int)(ShowDelta / ShowTime * 1000);

    [SerializeField] private Run _run;
    
    private TMP_Text _text;

    public void StartRun()
    {
        ChangeTextDilate(0, -1);
    }

    public void EndRun()
    {
        ChangeTextDilate(-1, 1);
    }
    
    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    private async void ChangeTextDilate(float startValue, int dir)
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