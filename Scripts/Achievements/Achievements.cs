using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

public class Achievements : MonoBehaviour
{
    [SerializeField] private List<AchievementData> _achievementsData;

    private DataStringArray _achievements;
    private HashSet<string> _givenAchievements;
    private Dictionary<string, DataInt> _achievementsProgress;
    private Dictionary<string, AchievementData> _achievementByName;

    public static Achievements Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            _achievements = new DataStringArray(nameof(Achievements));
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        _givenAchievements = new HashSet<string>(_achievements.Value);

        _achievementsProgress = new Dictionary<string, DataInt>();

        _achievementByName = new Dictionary<string, AchievementData>();
        for (var i = 0; i < _achievementsData.Count; i++)
        {
            _achievementByName[_achievementsData[i].Name] = _achievementsData[i];
        }
    }

    private void Start()
    {
        if (Instance == this)
        {
            if (!SteamManager.Initialized)
            {
                Debug.LogWarning("Steam manager don`t initialized, cannot grant previous stored achievements");
                return;
            }
            
            foreach (string achievement in _achievements.Value)
            {
                SteamUserStats.GetAchievement(achievement, out bool _);
            }

            SteamUserStats.StoreStats();
        }
    }

    public void GetAchievement(string achievementName)
    {
        if (_givenAchievements.Contains(achievementName)) return;

        _givenAchievements.Add(achievementName);
        _achievements.AddElement(achievementName);

        if (!SteamManager.Initialized)
        {
            Debug.LogWarning(
                "Steam Manager don`t initialized, achievement stored and will be granted on next session");
            return;
        }
        
        SteamUserStats.GetAchievement(achievementName, out bool _);
        SteamUserStats.StoreStats();
    }

    public void IncreaseProgress(string achievementName, int progress)
    {
        if (_givenAchievements.Contains(achievementName)) return;

        if (!_achievementByName.ContainsKey(achievementName))
        {
            throw new Exception($"{nameof(Achievements)} doesnt have data for {achievementName}");
        }

        int requiredProgress = _achievementByName[achievementName].RequiredProgress;

        DataInt currentProgress = _achievementsProgress.GetValueOrDefault(
            achievementName,
            new DataInt(achievementName, 0));

        if (currentProgress.Value + progress >= requiredProgress)
        {
            GetAchievement(achievementName);
        }
        else
        {
            currentProgress.Value += progress;
            _achievementsProgress[achievementName] = currentProgress;
        }
        
    }
}