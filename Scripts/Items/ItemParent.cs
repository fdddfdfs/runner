using UnityEngine;

public class ItemParent : MonoBehaviour
{
    [SerializeField] private ItemType _itemType;

    public ItemType ItemType => _itemType;

    public Item ItemObject { get; set; }
}