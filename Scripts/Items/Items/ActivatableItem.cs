﻿using System.Threading;
using StarterAssets;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

public abstract class ActivatableItem : Item
{
    private ActiveItemsUI _activeItemsUI;
    private ItemsActiveStates _itemsActiveStates;
    
    public void Init(ActiveItemsUI activeItemsUI, Run run, ItemsActiveStates itemsActiveStates)
    {
        base.Init(run);
        
        _activeItemsUI = activeItemsUI;
        _itemsActiveStates = itemsActiveStates;
    }

    public override void PickupItem(ThirdPersonController player)
    {
        base.PickupItem(player);

        _activeItemsUI.ShowNewItemEffect(ActiveItemType, ActiveTime);
        ActivateTime();
    }

    protected abstract float ActiveTime { get; }
    
    protected abstract ItemType ActiveItemType { get; }

    protected abstract void Activate();

    protected abstract void Deactivate();

    private async void ActivateTime()
    {
        _itemsActiveStates.ActivateItem(ActiveItemType);
        Activate();

        CancellationToken token = _itemsActiveStates.GetTokenForItem(ActiveItemType);

        await Task.Delay((int)ActiveTime * 1000, token).ContinueWith(GlobalCancellationToken.EmptyTask);

        if (token.IsCancellationRequested) return;
        
        Deactivate();
        _itemsActiveStates.DeactivateItem(ActiveItemType);
    }
}