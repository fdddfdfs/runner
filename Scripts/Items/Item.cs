using System.Collections;
using StarterAssets;
using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(MeshRenderer))]
public abstract class Item : MonoBehaviour
{
    private const float ActivateTime = 10;
    private const float DeactivateTime = 30;
    
    private MeshRenderer _meshRenderer;
    private BoxCollider _boxCollider;
    private bool _isAutoShowing;

    private WaitForSeconds _activateWaiter;
    private WaitForSeconds _deactivateWaiter;

    protected void Init(bool isAutoShowing = true, bool isAutoHiding = true)
    {
        _meshRenderer = GetComponent<MeshRenderer>();
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
        _meshRenderer.enabled = state;
        _boxCollider.enabled = state;
    }
}
