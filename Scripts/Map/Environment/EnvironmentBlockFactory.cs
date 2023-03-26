using System.Globalization;
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
        
        _count++;
        
        StaticBatchingUtility.Combine(environmentBlockObject);
        
        return environmentBlock;
    }
}