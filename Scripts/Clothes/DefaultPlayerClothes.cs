using System;
using System.Collections.Generic;
using UnityEngine;

public class DefaultPlayerClothes
{
    private readonly SkinnedMeshRenderer _playerMesh;
    private readonly Dictionary<ClotherType, Material> _defaultMaterials;
    private readonly Dictionary<ClotherType, int> _materialIndexes;

    public DefaultPlayerClothes(GameObject playerMeshParent, int chestMaterialNumber)
    {
        _playerMesh = playerMeshParent.GetComponentInChildren<SkinnedMeshRenderer>();

        if (_playerMesh == null)
        {
            throw new Exception($"Cannot find player SkinnedMeshRenderer in {playerMeshParent.name} object");
        }

        _defaultMaterials = new Dictionary<ClotherType, Material>
        {
            [ClotherType.Chest] = _playerMesh.materials[chestMaterialNumber],
        };
        
        _materialIndexes = new Dictionary<ClotherType, int>
        {
            [ClotherType.Chest] = chestMaterialNumber,
        };
    }

    public void ApplyDefaultClothes(InventoryDefaultClothesItemData clothesItemData)
    {
        if (!_materialIndexes.ContainsKey(clothesItemData.ClotherType))
        {
            throw new Exception($"Unknown material for ClotherType {clothesItemData.ClotherType}");
        }
        
        ChangePlayerMaterials(_materialIndexes[clothesItemData.ClotherType], clothesItemData.DefaultClothesMaterial);
    }

    public void ResetDefaultClothes(ClotherType clotherType)
    {
        if (_defaultMaterials.ContainsKey(clotherType))
        {
            ChangePlayerMaterials(_materialIndexes[clotherType], _defaultMaterials[clotherType]);
        }
    }

    private void ChangePlayerMaterials(int index, Material newMaterial)
    {
        Material[] materials = _playerMesh.materials;
        materials[index] = newMaterial;
        _playerMesh.materials = materials;
    }
}