using System.Collections.Generic;
using UnityEngine;

public class Environment : MapPart<EnvironmentBlockInfo, EnvironmentBlock>
{
    private const int EnvironmentBlocksGenerationCount = 10;
    
    public Environment(List<EnvironmentBlockInfo> blocks, Transform player):
        base(blocks, player, EnvironmentBlocksGenerationCount)
    {
        
    }

    protected override float GenerateBlock(EnvironmentBlockInfo block, EnvironmentBlock parent)
    {
        Object.Instantiate(block.Prefab, parent.transform);
        return block.Terrain.terrainData.size.z * block.Prefab.transform.localScale.z;
    }
}