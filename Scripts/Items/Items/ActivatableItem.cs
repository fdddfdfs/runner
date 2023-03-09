using System.Threading;
using StarterAssets;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

public abstract class ActivatableItem : Item
{
    private ActiveItemsUI _activeItemsUI;
    private ItemsActiveStates _itemsActiveStates;
    
    public void Init(
        ActiveItemsUI activeItemsUI,
        ICancellationTokenProvider cancellationTokenProvider,
        ItemsActiveStates itemsActiveStates)
    {
        base.Init(cancellationTokenProvider);
        
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
        CancellationToken endRunToken = _cancellationTokenProvider.GetCancellationToken();

        await AsyncUtils.Wait(ActiveTime, token);

        if (token.IsCancellationRequested && !endRunToken.IsCancellationRequested)
        {
            return;
        }

        Deactivate();
        _itemsActiveStates.DeactivateItem(ActiveItemType);
    }
}