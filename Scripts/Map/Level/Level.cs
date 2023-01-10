using System.Collections.Generic;
using UnityEngine;

public sealed class Level : MapPart<LevelBlockInfo, ObstacleBlock>
{
    private readonly Factories _factories;
    private readonly RunProgress _runProgress;
    private readonly List<LevelBlockInfo> _levelBlock;
    
    public Level(
        List<LevelBlockInfo> levelBlocks,
        Factories factories,
        Transform player,
        RunProgress runProgress) :
        base(levelBlocks, player)
    {
        _factories = factories;
        _runProgress = runProgress;
        _levelBlock = levelBlocks;
    }

    protected override Dictionary<int, FactoryPoolMono<ObstacleBlock>> InitializeBlockPools()
    {
        Dictionary<int, FactoryPoolMono<ObstacleBlock>> blockPools = new();
        for (var i = 0; i < _levelBlock.Count; i++)
        {
            AbstractFactory<ObstacleBlock> factory = new ObstacleBlockFactory(
                _levelBlock[i],
                _factories,
                _runProgress);
            blockPools.Add(i, new FactoryPoolMono<ObstacleBlock>(factory, null, true, 0));
        }

        return blockPools;
    }
}