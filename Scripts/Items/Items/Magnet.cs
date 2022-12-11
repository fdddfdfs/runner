using System;
using System.Collections;
using StarterAssets;
using UnityEngine;
using UnityEngine.UIElements;

public class Magnet : Item
{
    private const float ActiveTime = 10;

    private readonly Vector3 _overlapHalfSize = new(Level.ColumnOffset, 20, 2);
    private readonly Collider[] _overlappedColliders = new Collider[10];
    private Vector3 _center;
    private LayerMask _mask;
    private ActiveItemsUI _activeItemsUI;

    private static Coroutine _magnetActive;
    private static bool _isActive;

    public void Init(ActiveItemsUI activeItemsUI)
    {
        base.Init();

        _activeItemsUI = activeItemsUI;
        _center = new Vector3(0, 1, 0);
        _mask = LayerMask.GetMask("Item");
    }

    public override void PickupItem(ThirdPersonController player)
    {
        base.PickupItem(player);
        
        if (_isActive)
        {
            Coroutines.StopRoutine(_magnetActive);
            _isActive = false;
        }
        
        _activeItemsUI.ShowNewItemEffect(ItemType.Magnet, ActiveTime);
        _magnetActive = Coroutines.StartRoutine(MagnetActive(player));
    }

    private IEnumerator MagnetActive(ThirdPersonController player)
    {
        float currentTime = 0;
        _isActive = true;

        while (currentTime < ActiveTime)
        {
            var collidersCount = Physics.OverlapBoxNonAlloc(
                 _center + Vector3.forward * player.transform.position.z,
                _overlapHalfSize,
                _overlappedColliders,
                Quaternion.identity,
                _mask);
            
            for (int i = 0; i < collidersCount; i++)
            {
                if (_overlappedColliders[i].TryGetComponent(out Money money))
                {
                    money.MoveToPlayer(player);
                }
            }

            currentTime += Time.deltaTime;
            yield return null;
        }
    }
}