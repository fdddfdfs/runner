using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerClothes
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
        for (int i = 0; i < clothesIDs.Length; i++)
        {
            AddClothes(clothesIDs[i]);
        }
    }

    public void AddClothes(int clothID)
    {
        ClotherType clothType = InventoryAllItems.AllClothersType[clothID];
        
        for (var i = 0; i < _clothes.Count; i++)
        {
            if ((_clothes[i].Type & clothType) != 0)
            {
                if (_clothesIDs[i] == clothID) return;
                
                ClothesManager.RemoveClother(_clothes[i].Bones);
                _clothes.RemoveAt(i);
                _clothesIDs.RemoveAt(i);
                i--;
            }
        }

        ApplyCloth(clothID);
    }

    private void ApplyCloth(int clothID)
    {
        InventoryItem item = InventoryAllItems.AllItems[clothID];
        GameObject clother = Object.Instantiate(item.Prefab);
        ClotherType type = InventoryAllItems.AllClothersType[item.ID];
        List<GameObject> clotherBones = ClothesManager.ApplyClother(_player, _bones, clother, type);
        _clothes.Add((type,clotherBones));
        _clothesIDs.Add(clothID);
    }
}