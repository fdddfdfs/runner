using System.Threading;
using StarterAssets;

public abstract class ActivatableItem : Item
{
    private const int PickupItemEffectTimeMilliseconds = 5000;
    
    private ActiveItemsUI _activeItemsUI;
    private ItemsActiveStates _itemsActiveStates;
    private Effects _effects;
    
    public void Init(
        ActiveItemsUI activeItemsUI,
        ICancellationTokenProvider cancellationTokenProvider,
        ItemsActiveStates itemsActiveStates,
        Effects effects)
    {
        base.Init(cancellationTokenProvider);
        
        _activeItemsUI = activeItemsUI;
        _itemsActiveStates = itemsActiveStates;
        
        _effects = effects;
    }

    public override void PickupItem(ThirdPersonController player)
    {
        base.PickupItem(player);

        _activeItemsUI.ShowNewItemEffect(ActiveItemType, ActiveTime);
        ActivateTime();
        _effects.ActivateEffect(EffectType.PickupItem, transform.position, PickupItemEffectTimeMilliseconds);
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