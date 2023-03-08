using UnityEngine;

[CreateAssetMenu(fileName = "Environment", menuName = "Environments/EnvironmentBlockInfo")]
public sealed class EnvironmentBlockInfo : ScriptableObject, IWeightable
{
    [SerializeField] private int _weight;
    [SerializeField] private GameObject _prefab;

    private Terrain _terrain;
    
    public int Weight => _weight;
    
    public GameObject Prefab => _prefab;

    public Terrain Terrain
    {
        get
        {
            if (!_terrain)
            {
                _terrain = _prefab.GetComponentInChildren<Terrain>();
            }
            
            return _terrain;
        }
    }
}