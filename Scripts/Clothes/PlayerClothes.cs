using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public sealed class PlayerClothes
{
    private readonly GameObject _player;
    private readonly Transform[] _bones;
    private readonly List<(ClotherType Type,List<GameObject> Bones)> _clothes;
    private readonly List<int> _clothesIDs;

    private readonly DefaultPlayerClothes _defaultPlayerClothes;
    private readonly float _clothesScale;

    public IReadOnlyList<int> ClothesIDs => _clothesIDs;

    public PlayerClothes(
        Transform playerRootBone,
        GameObject playerMeshParent,
        int chestIndex,
        float clothesScale,
        IReadOnlyList<int> clothes)
    {
        _clothes = new List<(ClotherType, List<GameObject>)>();
        _clothesIDs = new List<int>();
        _player = playerRootBone.gameObject;
        _bones = ClothesManager.FindBones(playerRootBone);
        _clothesScale = clothesScale;

        for (var i = 0; i < clothes.Count; i++)
        {
            ApplyCloth(clothes[i]);
        }

        _defaultPlayerClothes = new DefaultPlayerClothes(playerMeshParent, chestIndex);
    }

    public PlayerClothes(Transform playerRootBone, GameObject playerMeshParent, int chestIndex, float clothesScale)
    {
        _clothes = new List<(ClotherType, List<GameObject>)>();
        _clothesIDs = new List<int>();
        _player = playerRootBone.gameObject;
        _bones = ClothesManager.FindBones(playerRootBone);
        _clothesScale = clothesScale;
        
        _defaultPlayerClothes = new DefaultPlayerClothes(playerMeshParent, chestIndex);
    }

    public void ChangeClothes(int[] clothesIDs, bool deleteIfExist)
    {
        for (var i = 0; i < clothesIDs.Length; i++)
        {
            ChangeClothes(clothesIDs[i], deleteIfExist);
        }
    }

    public void ChangeClothes(int clothesID, bool deleteIfExist)
    {
        if (deleteIfExist)
        {
            bool isDeleted = DeleteIfExist(clothesID);

            if (isDeleted)
            {
                ClothesStorage.PlayerClothes.Value = _clothesIDs.ToArray();
                return;
            }
        }

        var clothesData = InventoryAllItems.Instance.Items[clothesID] as InventoryClothesItemData;
        
        if (!clothesData)
        {
            throw new Exception($"Clothes ID: {clothesID} marked as clothes, but not clothes");
        }

        ClotherType clothesType = clothesData.ClotherType;
        
        for (var i = 0; i < _clothes.Count; i++)
        {
            if ((_clothes[i].Type & clothesType) != 0)
            {
                if (_clothesIDs[i] == clothesID) return;
                
                DeleteClother(i);
                
                i--;
            }
        }

        ApplyCloth(clothesID);
    }

    private bool DeleteIfExist(int clothesID)
    {
        int index = _clothesIDs.FindIndex(id => id == clothesID);

        if (index == -1) return false;
        
        DeleteClother(index);

        return true;
    }

    private void DeleteClother(int index)
    {
        if (_clothes[index].Bones != null)
        {
            ClothesManager.RemoveClother(_clothes[index].Bones);
        }
        else
        {
            _defaultPlayerClothes.ResetDefaultClothes(_clothes[index].Type);
        }
                
        _clothes.RemoveAt(index);
        _clothesIDs.RemoveAt(index);
    }

    private void ApplyCloth(int clothesID)
    {
        var clothesData = InventoryAllItems.Instance.Items[clothesID] as InventoryClothesItemData;
        
        if (!clothesData)
        {
            throw new Exception($"Clothes ID: {clothesID} marked as clothes, but not clothes");
        }

        switch (clothesData)
        {
            case InventoryNewClothesItemData newClothesItemData:
            {
                ClotherType clothesType = clothesData.ClotherType;
                GameObject clother = Object.Instantiate(newClothesItemData.Prefab);
                List<GameObject> clotherBones = ClothesManager.ApplyClother(
                    _player,
                    _bones,
                    clother,
                    clothesType,
                    _clothesScale);
                _clothes.Add((clothesType, clotherBones));

                break;
            }
            case InventoryDefaultClothesItemData defaultClothesItemData:
                _defaultPlayerClothes.ApplyDefaultClothes(defaultClothesItemData);
                _clothes.Add((defaultClothesItemData.ClotherType, null));
                break;
            default:
                throw new Exception("InventoryClothesItemData realization not implemented");
        }

        _clothesIDs.Add(clothesID);
        ClothesStorage.PlayerClothes.Value = _clothesIDs.ToArray();
    }
}