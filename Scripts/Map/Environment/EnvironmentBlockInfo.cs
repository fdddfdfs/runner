using UnityEngine;

[CreateAssetMenu(fileName = "Environment", menuName = "Environments/EnvironmentBlockInfo")]
public sealed class EnvironmentBlockInfo : ScriptableObject, IWeightable
{
    [SerializeField] private int _weight;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private AchievementData _achievementData;

    private Terrain _terrain;
    
    public int Weight => _weight;
    
    public GameObject Prefab => _prefab;

    public AchievementData AchievementData => _achievementData;

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