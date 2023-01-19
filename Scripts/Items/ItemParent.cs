using UnityEngine;

public class ItemParent : MonoBehaviour
{
    private const float Speed = 1;

    [SerializeField] protected ItemType _itemType;
    
    private readonly Quaternion _rotation = Quaternion.Euler(0,Speed,0);

    private Quaternion _startRotation;
    private Quaternion _startRotationOffset = Quaternion.identity;
    private bool _isEntered;

    public ItemType ItemType => _itemType;

    public Item ItemObject { get; set; }

    public void SetStartRotationOffset(Quaternion offset)
    {
        _startRotationOffset = offset;
    }

    public void EnterObstacle()
    {
        _isEntered = true;
        transform.localRotation = _startRotation * _startRotationOffset;
    }

    public void HideObstacle()
    {
        _isEntered = false;
    }

    private void Update()
    {
        if (_isEntered)
        {
            transform.localRotation *= _rotation;
        }
    }

    private void Awake()
    {
        _startRotation = transform.localRotation;
    }
}