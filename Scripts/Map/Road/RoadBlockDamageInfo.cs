using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[CreateAssetMenu(fileName = "RoadDamage", menuName = "Road/Damage")]
public sealed class RoadBlockDamageInfo : ScriptableObject, IWeightable
{
    [SerializeField] private GameObject _prefab;

    [UnityEngine.Min(0), Max(Map.ColumnOffset * Map.LinesCount)] 
    [SerializeField] private float _minScale;
    
    [UnityEngine.Min(0), Max(Map.ColumnOffset * Map.LinesCount)] 
    [SerializeField] private float _maxScale;

    [SerializeField] private int _weight;

    public GameObject Prefab => _prefab;

    public float MinScale => _minScale;

    public float MaxScale => _maxScale;
    
    public int Weight => _weight;
}