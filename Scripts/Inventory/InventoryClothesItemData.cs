using UnityEngine;

public abstract class InventoryClothesItemData : InventoryItemData
{
    [SerializeField] private ClotherType _clotherType;

    public ClotherType ClotherType => _clotherType;
    public override InventoryItemType InventoryItemType => InventoryItemType.Clothes;
}