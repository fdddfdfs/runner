using System.Collections;
using System.Collections.Generic;
using System.Linq;
using StarterAssets;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public abstract class Item : MonoBehaviour
{
    private const float ActivateTime = 1;
    private const float DeactivateTime = 30;
    
    private List<MeshRenderer> _meshRenderers;
    private BoxCollider _boxCollider;
    private bool _isAutoShowing;

    private WaitForSeconds _activateWaiter;
    private WaitForSeconds _deactivateWaiter;

    protected void Init(bool isAutoShowing = false, bool isAutoHiding = false)
    {
        _meshRenderers = GetComponentsInChildren<MeshRenderer>().ToList();
        _boxCollider = GetComponent<BoxCollider>();
        _isAutoShowing = isAutoShowing;

        if (isAutoHiding)
        {
            _deactivateWaiter ??= new WaitForSeconds(DeactivateTime);

            Coroutines.StartRoutine(DeactivateItem());
        }
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

    private IEnumerator DeactivateItem()
    {
        yield return _activateWaiter;

        gameObject.SetActive(false);
    }

    private void ChangeItemVisible(bool state)
    {
        foreach (MeshRenderer meshRenderer in _meshRenderers)
        {
            meshRenderer.enabled = state;            
        }

        _boxCollider.enabled = state;
    }
}
