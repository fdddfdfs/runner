using UnityEngine;

[CreateAssetMenu(fileName = "InventoryClothesItemData", menuName = "Inventory/InventoryClothesItemData")]
public class InventoryClothesItemData : InventoryItemData
{
    [SerializeField] private ClotherType _clotherType;

    public ClotherType ClotherType => _clotherType;
    public override InventoryItemType InventoryItemType => InventoryItemType.Clothes;
}