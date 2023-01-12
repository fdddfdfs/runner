using UnityEngine;

[System.Serializable]
public sealed class Line
{
    [SerializeField] private GameObject _obstacle1;
    [SerializeField] private GameObject _obstacle2;
    [SerializeField] private GameObject _obstacle3;
    [SerializeField] private bool _needSpawnItems1;
    [SerializeField] private bool _needSpawnItems2;
    [SerializeField] private bool _needSpawnItems3;

    public bool NeedSpawnItems1 => _needSpawnItems1;

    public bool NeedSpawnItems2 => _needSpawnItems2;

    public bool NeedSpawnItems3 => _needSpawnItems3;

    public GameObject Obstacle1 => _obstacle1;

    public GameObject Obstacle2 => _obstacle2;

    public GameObject Obstacle3 => _obstacle3;
}
