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

    public IReadOnlyList<int> ClothesIDs => _clothesIDs;

    public PlayerClothes(Transform playerRootBone, IReadOnlyList<int> clothes)
    {
        _clothes = new List<(ClotherType, List<GameObject>)>();
        _clothesIDs = new List<int>();
        _player = playerRootBone.gameObject;
        _bones = ClothesManager.FindBones(playerRootBone);

        for (var i = 0; i < clothes.Count; i++)
        {
            ApplyCloth(clothes[i]);
        }
    }

    public PlayerClothes(Transform playerRootBone)
    {
        _clothes = new List<(ClotherType, List<GameObject>)>();
        _clothesIDs = new List<int>();
        _player = playerRootBone.gameObject;
        _bones = ClothesManager.FindBones(playerRootBone);
    }

    public void AddClothes(int[] clothesIDs)
    {
        for (var i = 0; i < clothesIDs.Length; i++)
        {
            AddClothes(clothesIDs[i]);
        }
    }

    public void AddClothes(int clothesID)
    {
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
                
                ClothesManager.RemoveClother(_clothes[i].Bones);
                _clothes.RemoveAt(i);
                _clothesIDs.RemoveAt(i);
                i--;
            }
        }

        ApplyCloth(clothesID);
    }

    private void ApplyCloth(int clothesID)
    {
        var clothesData = InventoryAllItems.Instance.Items[clothesID] as InventoryClothesItemData;
        
        if (!clothesData)
        {
            throw new Exception($"Clothes ID: {clothesID} marked as clothes, but not clothes");
        }

        ClotherType clothesType = clothesData.ClotherType;
        GameObject clother = Object.Instantiate(clothesData.Prefab);
        List<GameObject> clotherBones = ClothesManager.ApplyClother(_player, _bones, clother, clothesType);
        _clothes.Add((clothesType, clotherBones));
        _clothesIDs.Add(clothesID);
    }
}