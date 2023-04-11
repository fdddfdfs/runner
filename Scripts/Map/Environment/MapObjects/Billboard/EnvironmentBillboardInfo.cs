using UnityEngine;

[CreateAssetMenu(fileName = "EnvironmentBillboard", menuName = "Environments/EnvironmentBillboardInfo")]
public class EnvironmentBillboardInfo : ScriptableObject
{
    [SerializeField] private Sprite _sprite;
    [SerializeField] private AchievementData _billboardAchievement;

    public Sprite Sprite => _sprite;
    public AchievementData BillboardAchievement => _billboardAchievement;
}