using UnityEngine;

[CreateAssetMenu(fileName = "InventoryNewClothesItemData", menuName = "Inventory/InventoryNewClothesItemData")]
public sealed class InventoryNewClothesItemData : InventoryClothesItemData
{
    [SerializeField] private GameObject _prefab;

    public GameObject Prefab => _prefab;
}