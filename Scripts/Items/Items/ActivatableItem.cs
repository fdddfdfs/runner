using System.Collections;
using StarterAssets;
using UnityEngine;

public abstract class ActivatableItem<T> : Item
{
    private WaitForSeconds _waiter;
    private ActiveItemsUI _activeItemsUI;

    private static Coroutine _activateRoutine;
    private static bool _isActive;
    
    public void Init(ActiveItemsUI activeItemsUI)
    {
        base.Init();

        _waiter = new WaitForSeconds(ActiveTime);
        _activeItemsUI = activeItemsUI;
    }

    public override void PickupItem(ThirdPersonController player)
    {
        base.PickupItem(player);

        if (_isActive)
        {
            Coroutines.StopRoutine(_activateRoutine);
            Deactivate();
        }

        _activeItemsUI.ShowNewItemEffect(ActiveItemType, ActiveTime);
        _activateRoutine = Coroutines.StartRoutine(ActivateItem());
    }

    protected abstract float ActiveTime { get; }
    
    protected abstract ItemType ActiveItemType { get; }

    protected abstract void Activate();

    protected abstract void Deactivate();
    
    private IEnumerator ActivateItem()
    {
        _isActive = true;
        Activate();
        
        yield return _waiter;

        Deactivate();
        _isActive = false;
    }
}