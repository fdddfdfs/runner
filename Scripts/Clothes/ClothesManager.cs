using System.Collections.Generic;
using UnityEngine;

public class ClothesManager : MonoBehaviour
{
    private const string BONE_NAME = "petya";

    public static List<GameObject> ApplyClother(
        GameObject character,
        Transform[] characterBones,
        GameObject clother,
        ClotherType type,
        float scale = 1)
    {
        Transform[] clotherBones = FindBones(clother, out Transform clotherBone);

        clotherBone.SetParent(null);

        List<GameObject> clotherParts = new();

        for (var i = 0; i < characterBones.Length; i++)
        {
            clotherBones[i].DetachChildren();
            clotherBones[i].SetParent(characterBones[i]);

            clotherBones[i].localPosition = new Vector3(0, 0, 0);
            clotherBones[i].localRotation = new Quaternion(0, 0, 0, 0);
            if (type != ClotherType.Head && type != ClotherType.Face && type != (ClotherType.Head | ClotherType.Face))
            {
                clotherBones[i].localScale = new Vector3(scale, scale, scale);
            }
            else
            {
                clotherBones[i].localScale = new Vector3(1, 1, 1);
            }

            clotherBones[i].name = "i";
            clotherParts.Add(clotherBones[i].gameObject);
        }

        clother.transform.SetParent(character.transform);
        clotherParts.Add(clother);

        return clotherParts;
    }

    public static void RemoveClother(List<GameObject> clotherParts)
    {
        for (var i = 0; i < clotherParts.Count; i++)
        {
            clotherParts[i].transform.SetParent(null);
            Destroy(clotherParts[i].gameObject);
        }

        clotherParts.Clear();
    }

    public static Transform[] FindBones(GameObject obj, out Transform rootBone)
    {
        rootBone = null;

        for (var i = 0; i < obj.transform.childCount; i++)
        {
            if (obj.transform.GetChild(i).name.StartsWith(BONE_NAME))
            {
                rootBone = obj.transform.GetChild(i);
                break;
            }
        }

        if (rootBone == null)
        {
            Debug.LogError($"ERROR WITH FINDING BONE NAMED {BONE_NAME} ON OBJECT {obj.name}");
            return null;
        }

        Transform[] objBones = rootBone.GetComponentsInChildren<Transform>();

        return objBones;
    }

    public static Transform[] FindBones(Transform rootBone)
    {
        return rootBone.GetComponentsInChildren<Transform>();
    }

    public static List<GameObject> LoadClothesByIDs(GameObject character, int[] clothes, float scale = 1)
    {
        Transform[] characterBones = FindBones(character, out Transform rootBone);
        List<GameObject> clothesParts = new();

        for (int i = 0; i < clothes.Length; i++)
        {
            List<GameObject> appliedClotherParts = ApplyClother(
                character,
                characterBones,
                Instantiate(InventoryAllItems.AllItems[clothes[i]].Prefab),
                InventoryAllItems.AllClothersType[InventoryAllItems.AllItems[clothes[i]].ID],
                scale);
            
            clothesParts.AddRange(appliedClotherParts);
        }

        return clothesParts;
    }

    public static List<int> SetRandomClothes(GameObject character, float scale = 1)
    {
        Transform[] characterBones = FindBones(character, out Transform rootBone);

        List<int> clothesIDs = new ();

        foreach (ClotherType item in (ClotherType[])System.Enum.GetValues(typeof(ClotherType)))
        {
            if (InventoryAllItems.AllClothersByType[item].Count == 0)
            {
                break;
            }

            int r = Random.Range(0, InventoryAllItems.AllClothersByType[item].Count);
            ApplyClother(
                character,
                characterBones,
                Instantiate(InventoryAllItems.AllItems[InventoryAllItems.AllClothersByType[item][r]].Prefab),
                item,
                scale);

            clothesIDs.Add(InventoryAllItems.AllClothersByType[item][r]);
        }

        return clothesIDs;
    }
}