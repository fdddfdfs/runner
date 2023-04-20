using System.Collections.Generic;
using UnityEngine;

public sealed class Environment : MapPart<EnvironmentBlockInfo, EnvironmentBlock>
{
    private const float NextBlockDistancePercentToHide = 0.6f;
    
    private readonly List<EnvironmentBlockInfo> _environmentBlocks;
    
    public Environment(List<EnvironmentBlockInfo> environmentBlocks, Transform player)
        : base(environmentBlocks, player, false, NextBlockDistancePercentToHide, 0f)
    {
        _environmentBlocks = environmentBlocks;
    }

    protected override Dictionary<int, FactoryPoolMono<EnvironmentBlock>> InitializeBlockPools()
    {
        Dictionary<int, FactoryPoolMono<EnvironmentBlock>> blockPools = new();
        for (var i = 0; i < _environmentBlocks.Count; i++)
        {
            AbstractFactory<EnvironmentBlock> factory = new EnvironmentBlockFactory(_environmentBlocks[i]);
            blockPools.Add(i, new FactoryPoolMono<EnvironmentBlock>(factory, null, true, 0));
        }

        return blockPools;
    }
}