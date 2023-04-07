using System.Globalization;
using Gaia;
using UnityEngine;

public sealed class EnvironmentBlockFactory : AbstractFactory<EnvironmentBlock>
{
    private readonly EnvironmentBlockInfo _environmentBlockInfo;
    private int _count;
    
    public EnvironmentBlockFactory(EnvironmentBlockInfo environmentBlockInfo)
    {
        _environmentBlockInfo = environmentBlockInfo;
    }
    
    public override EnvironmentBlock CreateItem()
    {
        GameObject environmentBlockObject = Object.Instantiate(_environmentBlockInfo.Prefab);
        environmentBlockObject.name = 
            $"EnvironmentBlock_{_environmentBlockInfo.name}_{_count.ToString(CultureInfo.InvariantCulture)}";
        
        var environmentBlock = environmentBlockObject.GetComponent<EnvironmentBlock>();
        environmentBlock ??= environmentBlockObject.AddComponent<EnvironmentBlock>();
        environmentBlock.Init(
            _environmentBlockInfo.Terrain.terrainData.size.z *
            _environmentBlockInfo.Prefab.transform.localScale.z,
            _environmentBlockInfo.AchievementData);

        var terrainDetailOverwrite = environmentBlockObject.GetComponentInChildren<TerrainDetailOverwrite>();

        if (terrainDetailOverwrite != null)
        {
            terrainDetailOverwrite.m_detailDensity = 0.3f + SettingsStorage.Graphic.Value * 0.1f;
            terrainDetailOverwrite.m_detailDistance = 115 + SettingsStorage.Graphic.Value * 15;
            terrainDetailOverwrite.m_detailQuality = (GaiaConstants.TerrainDetailQuality)SettingsStorage.Graphic.Value;
            terrainDetailOverwrite.ApplySettings(false);
        }

        _count++;
        
        StaticBatchingUtility.Combine(environmentBlockObject);
        
        return environmentBlock;
    }
}