using System;
using DG.Tweening;
using StarterAssets;
using UnityEngine;

public sealed class Money : Item
{
    private const float Speed = 0.5f;
    private const float MoneyStopMoveRadius = 1f;
    private const float SquaredMoneyStopMoveRadius = MoneyStopMoveRadius * MoneyStopMoveRadius;

    private Vector3 _startPosition;
    private RunProgress _runProgress;
    
    private readonly Action<Money, Transform, Tweener> _tweenerOnUpdate = (money, playerTransform, tweener) =>
    {
        if (Vector3.SqrMagnitude(money.transform.position - playerTransform.position) > SquaredMoneyStopMoveRadius)
        {
            tweener.ChangeEndValue(playerTransform.position, true);
        }
        else
        {
            money.PickupItem(null);
            money.ChangeColliderActive(true);
        }
    };

    public static int Weight => 20;

    public void Init(RunProgress runProgress, Run run, bool isAutoShowing)
    {
        base.Init(run, isAutoShowing);
        
        _runProgress = runProgress;
    }
    
    public override void PickupItem(ThirdPersonController player)
    {
        base.PickupItem(player);
        
        _runProgress.AddMoney();
        
        if (_startPosition != default)
        {
            transform.position = _startPosition;
            _startPosition = default;
            transform.DOKill();
        }
    }

    public void MoveToPlayer(ThirdPersonController player)
    {
        _startPosition = transform.position;
        ChangeColliderActive(false);

        Tweener tweener = transform.DOMove(
            player.transform.position,
            Speed / _runProgress.SpeedMultiplier).SetEase(Ease.OutExpo);
        
        tweener.onUpdate += () => _tweenerOnUpdate(this, player.transform, tweener);
    }
}
