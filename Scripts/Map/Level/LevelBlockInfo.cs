using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Levels")]
public sealed class LevelBlockInfo : ScriptableObject , IWeightable
{
    [SerializeField] private List<Line> _line;
    [SerializeField] private int _weight;

    public List<Line> Line => _line;
    
    public int Weight => _weight;
}
