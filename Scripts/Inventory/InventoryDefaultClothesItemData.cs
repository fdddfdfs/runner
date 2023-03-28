using UnityEngine;

[CreateAssetMenu(fileName = "InventoryDefaultClothesItemData", menuName = "Inventory/InventoryDefaultClothesItemData")]
public sealed class InventoryDefaultClothesItemData : InventoryClothesItemData
{
    [SerializeField] private Material _defaultClothesMaterial;

    public Material DefaultClothesMaterial => _defaultClothesMaterial;
}