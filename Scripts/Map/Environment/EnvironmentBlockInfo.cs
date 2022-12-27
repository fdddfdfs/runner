using UnityEngine;

[CreateAssetMenu(fileName = "Environment", menuName = "Environments")]
public class EnvironmentBlockInfo : ScriptableObject, IWeightable
{
    [SerializeField] private int _weight;
    [SerializeField] private Terrain _terrain;

    public int Weight => _weight;
    
    public GameObject Prefab => _terrain.gameObject;

    public Terrain Terrain => _terrain;
}