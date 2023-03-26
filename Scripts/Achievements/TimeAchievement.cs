using System.Collections.Generic;
using UnityEngine;

public class TimeAchievement : MonoBehaviour
{
    private const int TriggerAchievementDelta = 1;
    
    [SerializeField] private List<AchievementData> _timeAchievements;

    private float _currentTime;

    private void FixedUpdate()
    {
        _currentTime += Time.fixedDeltaTime * AsyncUtils.TimeScale;

        if (_currentTime < TriggerAchievementDelta) return;
        
        foreach (AchievementData timeAchievement in _timeAchievements)
        {
            Achievements.Instance.IncreaseProgress(timeAchievement.Name, TriggerAchievementDelta);
        }

        _currentTime -= TriggerAchievementDelta;
    }
}