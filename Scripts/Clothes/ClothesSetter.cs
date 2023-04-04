using System.Collections.Generic;
using UnityEngine;

public class ClothesSetter : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private List<GameObject> _cloth;

    private void Awake()
    {
        Transform[] bones = ClothesManager.FindBones(_player.gameObject, out Transform _);
        foreach (GameObject cloth in _cloth)
        {
            ClothesManager.ApplyClother(_player.gameObject, bones, cloth, ClotherType.Chest);
        }
    }
}