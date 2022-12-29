using DG.Tweening;
using StarterAssets;
using UnityEngine;

public class Money : Item
{
    private const float Speed = 0.5f;
    private const float MoneyStopMoveRadius = 1f;

    private Vector3 _startPosition;
    private RunProgress _runProgress;
    
    public void Init(RunProgress runProgress, bool isAutoActivating, bool isAutoHiding)
    {
        base.Init(isAutoActivating, isAutoHiding);
        
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
            Speed/_runProgress.SpeedMultiplayer).SetEase(Ease.OutExpo);
        tweener.onUpdate += () =>
        {
            if (Vector3.Magnitude(transform.position - player.transform.position) > MoneyStopMoveRadius)
            {
                tweener.ChangeEndValue(player.transform.position, true);
            }
            else
            {
                PickupItem(null);
            }
        };
    }
}
