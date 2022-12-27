using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public sealed class Line
{
    [SerializeField] private GameObject _obstacle1;
    [SerializeField] private GameObject _obstacle2;
    [SerializeField] private GameObject _obstacle3;
    [SerializeField] private ItemType _itemType1;
    [SerializeField] private ItemType _itemType2;
    [SerializeField] private ItemType _itemType3;

    public ItemType ItemType1 => _itemType1;

    public ItemType ItemType2 => _itemType2;

    public ItemType ItemType3 => _itemType3;

    public GameObject Obstacle1 => _obstacle1;

    public GameObject Obstacle2 => _obstacle2;

    public GameObject Obstacle3 => _obstacle3;
}
