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
    private const int DeactivateTimeMilliseconds = 20 * 1000;

    private List<MeshRenderer> _meshRenderers;
    private BoxCollider _boxCollider;
    private bool _isAutoShowing;

    protected ICancellationTokenProvider _cancellationTokenProvider;
    private CancellationTokenSource _cancellationTokenSource;
    private CancellationToken[] _linkedTokens;
    private bool _isDeactivating;

    protected void Init(ICancellationTokenProvider cancellationTokenProvider, bool isAutoShowing = false)
    {
        _meshRenderers = GetComponentsInChildren<MeshRenderer>().ToList();
        _boxCollider = GetComponent<BoxCollider>();
        _isAutoShowing = isAutoShowing;
        _cancellationTokenProvider = cancellationTokenProvider;

        _linkedTokens = new []{cancellationTokenProvider.GetCancellationToken()};
    }

    public virtual void PickupItem(ThirdPersonController player)
    {
        if (_isAutoShowing)
        {
            ChangeItemVisible(false);
            
            ActivateItem();
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

    private async void ActivateItem()
    {
        await AsyncUtils.Wait(ActivateTime, _cancellationTokenSource.Token);
        
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
            _cancellationTokenSource?.Dispose();
            _linkedTokens[0] = _cancellationTokenProvider.GetCancellationToken();
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(_linkedTokens);
        }

        await AsyncUtils.Wait(DeactivateTimeMilliseconds, _cancellationTokenSource.Token);

        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }

        _isDeactivating = false;
    }
}
