using System.Collections.Generic;
using System.Threading;
using StarterAssets;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

public sealed class Magnet : Item
{
    private const float AddedSecondsPerLevel = 2;
    private const float BaseDuration = 10;

    private readonly Vector3 _overlapHalfSize = new(Map.ColumnOffset, 20, 2);
    private readonly Collider[] _overlappedColliders = new Collider[10];
    private readonly HashSet<Collider> _movingMoneys = new ();

    private ItemsActiveStates _itemsActiveStates;

    private Vector3 _center;
    private LayerMask _mask;
    private ActiveItemsUI _activeItemsUI;

    public static int Weight => 1;

    private static ItemType ActiveItemType => ItemType.Magnet;
    
    private static float ActiveTime => BaseDuration + AddedSecondsPerLevel * Stats.MagnetLevel.Value;

    public void Init(
        ActiveItemsUI activeItemsUI,
        ICancellationTokenProvider cancellationTokenProvider,
        ItemsActiveStates itemsActiveStates)
    {
        base.Init(cancellationTokenProvider);

        _activeItemsUI = activeItemsUI;
        _center = new Vector3(0, 0, 0);
        _mask = LayerMask.GetMask("Item");

        _itemsActiveStates = itemsActiveStates;
    }

    public override void PickupItem(ThirdPersonController player)
    {
        Sounds.Instance.PlayRandomSounds(2, "Item");
        Achievements.Instance.GetAchievement("Item_6");
        
        base.PickupItem(player);

        MagnetActive(player);
    }

    private async void MagnetActive(ThirdPersonController player)
    {
        float currentTime = 0;
        _activeItemsUI.ShowNewItemEffect(ItemType.Magnet, ActiveTime);
        
        _itemsActiveStates.ActivateItem(ActiveItemType);
        CancellationToken token = _itemsActiveStates.GetTokenForItem(ActiveItemType);
        CancellationToken endRunToken = _cancellationTokenProvider.GetCancellationToken();

        _movingMoneys.Clear();
        
        while (currentTime < ActiveTime)
        {
            Vector3 position = player.transform.position;
            int collidersCount = Physics.OverlapBoxNonAlloc(
                 _center + Vector3.up * position.y + Vector3.forward * position.z,
                _overlapHalfSize,
                _overlappedColliders,
                Quaternion.identity,
                _mask);

            for (var i = 0; i < collidersCount; i++)
            {
                if (_movingMoneys.Contains(_overlappedColliders[i])) continue;

                _movingMoneys.Add(_overlappedColliders[i]);
                
                if (_overlappedColliders[i].TryGetComponent(out Money money))
                {
                    money.MoveToPlayerAsync(player);
                }
            }

            currentTime += Time.unscaledDeltaTime * AsyncUtils.TimeScale;
            await Task.Yield();
            if(token.IsCancellationRequested || endRunToken.IsCancellationRequested) break;
        }

        if (token.IsCancellationRequested && !endRunToken.IsCancellationRequested)
        {
            return;
        }
        
        _itemsActiveStates.DeactivateItem(ActiveItemType);
    }
}