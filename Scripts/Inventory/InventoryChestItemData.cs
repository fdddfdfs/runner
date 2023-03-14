using UnityEngine;

[CreateAssetMenu(fileName = "InventoryChestItemData", menuName = "Inventory/InventoryChestItemData")]
public sealed class InventoryChestItemData : InventoryItemData
{
    public override InventoryItemType InventoryItemType => InventoryItemType.Chest;
}