﻿public sealed class EnvironmentBlock : Triggerable, IMapBlock
{
    private AchievementData _achievementData;
    
    public float BlockSize { get; private set; }

    public void Init(float blockSize, AchievementData achievementData)
    {
        BlockSize = blockSize;

        _achievementData = achievementData;
        
        base.Init();
    }

    public void EnterBlock()
    {
        Achievements.Instance.GetAchievement(_achievementData.Name);
    }

    public async void HideBlock()
    {
        await AsyncUtils.Wait(0.1f, AsyncUtils.Instance.GetCancellationToken());
        
        gameObject.SetActive(false);
    }
}