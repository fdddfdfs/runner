using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StarterAssets;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public abstract class Item : MonoBehaviour
{
    private const float ActivateTime = 1;
    private const int DeactivateTime = 20 * 1000;

    private List<MeshRenderer> _meshRenderers;
    private BoxCollider _boxCollider;
    private bool _isAutoShowing;

    private Run _run;
    private CancellationTokenSource _cancellationTokenSource;
    private bool _isDeactivating;

    private WaitForSeconds _activateWaiter;

    protected void Init(Run run, bool isAutoShowing = false)
    {
        _meshRenderers = GetComponentsInChildren<MeshRenderer>().ToList();
        _boxCollider = GetComponent<BoxCollider>();
        _isAutoShowing = isAutoShowing;
        _run = run;
    }

    public virtual void PickupItem(ThirdPersonController player)
    {
        if (_isAutoShowing)
        {
            ChangeItemVisible(false);

            _activateWaiter ??= new WaitForSeconds(ActivateTime);
            
            Coroutines.StartRoutine(ActivateItem());
        }
        else
        {
            gameObject.SetActive(false);

            if (_isDeactivating)
            {
                _cancellationTokenSource?.Cancel();
            }
        }
    }

    protected void ChangeColliderActive(bool state)
    {
        _boxCollider.enabled = state;
    }

    private IEnumerator ActivateItem()
    {
        yield return _activateWaiter;
        
        ChangeItemVisible(true);
    }

    private void ChangeItemVisible(bool state)
    {
        foreach (MeshRenderer meshRenderer in _meshRenderers)
        {
            meshRenderer.enabled = state;            
        }

        _boxCollider.enabled = state;
    }

    public async void DeactivateInTime()
    {
        _isDeactivating = true;
        
        if (_cancellationTokenSource == null || _cancellationTokenSource.IsCancellationRequested)
        {
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(_run.EndRunToken);
        }
        
        await Task.Delay(DeactivateTime, _cancellationTokenSource.Token)
            .ContinueWith(GlobalCancellationToken.EmptyTask);

        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }

        _isDeactivating = false;
    }
}
