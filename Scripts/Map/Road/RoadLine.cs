using System;
using UnityEngine;

[Serializable]
public class RoadLine
{
    [SerializeField] private GameObject _leftBorder;
    [SerializeField] private GameObject _road;
    [SerializeField] private GameObject _rightBorder;

    public GameObject LeftBorder => _leftBorder;

    public GameObject Road => _road;

    public GameObject RightBorder => _rightBorder;
}