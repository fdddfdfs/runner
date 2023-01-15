using UnityEngine;

[CreateAssetMenu(fileName = "Road", menuName = "Road")]
public sealed class RoadBlockInfo : ScriptableObject, IWeightable
{
    [SerializeField] private RoadLine _roadLine;
    [SerializeField] private int _weight;

    public int Weight => _weight;
    
    public RoadLine RoadLine => _roadLine;
}