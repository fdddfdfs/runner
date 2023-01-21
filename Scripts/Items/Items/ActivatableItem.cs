using System.Collections;
using System.Threading;
using StarterAssets;
using UnityEditor.VersionControl;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

public abstract class ActivatableItem : Item
{
    private ActiveItemsUI _activeItemsUI;
    
    private bool _isActive;
    private CancellationTokenSource _cancellationTokenSource;
    
    public void Init(ActiveItemsUI activeItemsUI, Run run)
    {
        base.Init(run);
        
        _activeItemsUI = activeItemsUI;
        _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(_run.EndRunToken);
    }

    public override void PickupItem(ThirdPersonController player)
    {
        base.PickupItem(player);

        if (_isActive)
        {
            _cancellationTokenSource.Cancel();
        }

        if (_cancellationTokenSource.IsCancellationRequested)
        {
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(_run.EndRunToken);
        }

        _activeItemsUI.ShowNewItemEffect(ActiveItemType, ActiveTime);
        ActivateTime();
    }

    protected abstract float ActiveTime { get; }
    
    protected abstract ItemType ActiveItemType { get; }

    protected abstract void Activate();

    protected abstract void Deactivate();

    private async void ActivateTime()
    {
        _isActive = true;
        Activate();

        await Task.Delay((int)ActiveTime * 1000, _run.EndRunToken).ContinueWith(GlobalCancellationToken.EmptyTask);

        Deactivate();
        _isActive = false;
    }
}