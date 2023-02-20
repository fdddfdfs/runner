using UnityEngine;

public abstract class ColliderVisual : Visual
{
    [SerializeField] private BoxCollider _boxCollider;

    private Vector3 _colliderOffset;
    
    private Vector3 ColliderOffset
    {
        get
        {
            if (_colliderOffset == default)
            {
                _colliderOffset = new Vector3(0, _boxCollider.size.y / 2, 0);
            }

            return _colliderOffset;
        }
    }
    
    public override void ChangeActiveState(bool state)
    {
        base.ChangeActiveState(state);

        _player.PlayerBones.position += state ? ColliderOffset : -_colliderOffset;
    }

    private void Awake()
    {
        _boxCollider.isTrigger = true;
    }
}