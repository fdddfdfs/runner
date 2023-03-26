using UnityEngine;

[CreateAssetMenu(fileName = "AchievementData", menuName = "Achievements/AchievementData")]
public class AchievementData : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private int _requiredProgress;

    public string Name => _name;
    public int RequiredProgress => _requiredProgress;
}