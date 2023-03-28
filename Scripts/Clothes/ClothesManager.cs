using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class ClothesManager : MonoBehaviour
{
    private const string BONE_NAME = "mixamorig:Hips";

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
            clotherBones[i].localScale = new Vector3(scale, scale, scale);

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
            Transform child = obj.transform.GetChild(i);
            if (child.name.StartsWith(BONE_NAME))
            {
                rootBone = child;
                break;
            }

            for (int j = 0; j < child.transform.childCount; j++)
            {
                if (child.transform.GetChild(j).name.StartsWith(BONE_NAME))
                {
                    rootBone = child.transform.GetChild(j);
                    break;
                }
            }
        }

        if (rootBone == null)
        {
            Debug.LogError($"ERROR WITH FINDING BONE NAMED {BONE_NAME} ON OBJECT {obj.name}");
            return null;
        }

        Transform[] objBones = rootBone.GetComponentsInChildren<Transform>();
        objBones = objBones.Append(rootBone).ToArray();

        return objBones;
    }

    public static Transform[] FindBones(Transform rootBone)
    {
        return rootBone.GetComponentsInChildren<Transform>();
    }

    /*public static List<GameObject> LoadClothesByIDs(GameObject character, int[] clothes, float scale = 1)
    {
        Transform[] characterBones = FindBones(character, out Transform rootBone);
        List<GameObject> clothesParts = new();

        for (int i = 0; i < clothes.Length; i++)
        {
            var clothesData = InventoryAllItems.Instance.Items[clothes[i]] as InventoryClothesItemData;

            if (!clothesData)
            {
                throw new Exception($"Clothes ID: {clothes[i]} marked as clothes, but not clothes");
            }
            
            List<GameObject> appliedClotherParts = ApplyClother(
                character,
                characterBones,
                Instantiate(clothesData.Prefab),
                clothesData.ClotherType,
                scale);
            
            clothesParts.AddRange(appliedClotherParts);
        }

        return clothesParts;
    }*/
}
