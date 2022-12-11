using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public abstract class Item : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    private BoxCollider _boxCollider;

    private readonly WaitForSeconds _waiter = new(10);
    
    public virtual void Init()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _boxCollider = GetComponent<BoxCollider>();
    }
    
    public virtual void PickupItem(ThirdPersonController player)
    {
        ChangeItemVisible(false);

        Coroutines.StartRoutine(ActivateItem());
    }

    protected void ChangeColliderActive(bool state)
    {
        _boxCollider.enabled = state;
    }

    private IEnumerator ActivateItem()
    {
        yield return _waiter;
        
        ChangeItemVisible(true);
    }

    protected void ChangeItemVisible(bool state)
    {
        _meshRenderer.enabled = state;
        _boxCollider.enabled = state;
    }
}
