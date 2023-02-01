using System;
using System.Threading;
using System.Threading.Tasks;
using DG.Tweening;
using StarterAssets;
using UnityEngine;

public sealed class Money : Item
{
    private const float Speed = 0.5f;

    private Vector3 _startPosition;
    private RunProgress _runProgress;
    private bool _isPicked;

    public static int Weight => 20;

    public void Init(RunProgress runProgress, Run run, bool isAutoShowing)
    {
        base.Init(run, isAutoShowing);
        
        _runProgress = runProgress;
        _startPosition = transform.localPosition;
    }
    
    public override void PickupItem(ThirdPersonController player)
    {
        if (_isPicked) return;

        _isPicked = true;
        
        base.PickupItem(player);
        
        _runProgress.AddMoney();
        
        transform.localPosition = _startPosition;
    }

    public async void MoveToPlayerAsync(ThirdPersonController player)
    {
        float currentTime = 0;
        Vector3 currentPosition = transform.position;
        Transform endTransform = player.transform;
        CancellationToken token = _run.EndRunToken;
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
