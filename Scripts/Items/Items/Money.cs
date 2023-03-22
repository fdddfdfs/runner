using System.Threading;
using System.Threading.Tasks;
using StarterAssets;
using UnityEngine;

public sealed class Money : Item
{
    private const float Speed = 0.5f;

    private Vector3 _startPosition;
    private RunProgress _runProgress;
    private bool _isPicked;
    private Effects _effects;

    public static int Weight => 20;
    
    public void Init(
        RunProgress runProgress,
        ICancellationTokenProvider cancellationTokenProvider,
        bool isAutoShowing,
        Effects effects)
    {
        base.Init(cancellationTokenProvider, isAutoShowing);
        
        _runProgress = runProgress;
        _startPosition = transform.localPosition;
        _effects = effects;
    }
    
    public override void PickupItem(ThirdPersonController player)
    {
        if (_isPicked) return;

        _isPicked = true;
        
        base.PickupItem(player);
        
        _runProgress.AddMoney();
        
        _effects.ActivateEffect(EffectType.PickupMoney, transform.position);
        
        transform.localPosition = _startPosition;
        
        Sounds.Instance.PlaySound(0, "PickupMoney");
    }

    public async void MoveToPlayerAsync(ThirdPersonController player)
    {
        float currentTime = 0;
        Vector3 currentPosition = transform.position;
        Transform endTransform = player.transform;
        CancellationToken token = _cancellationTokenProvider.GetCancellationToken();
        while (currentTime < Speed)
        {
            transform.position = Vector3.Lerp(currentPosition, endTransform.position, currentTime / Speed);
            currentTime += Time.deltaTime;
            await Task.Yield();

            if (token.IsCancellationRequested || _isPicked)
            {
                break;
            }
        }
        
        PickupItem(player);
    }

    private void OnEnable()
    {
        _isPicked = false;
    }
}
