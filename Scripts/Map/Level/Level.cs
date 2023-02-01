﻿using System.Collections.Generic;
using UnityEngine;

public sealed class Level : MapPart<LevelBlockInfo, LevelBlock>
{
    private readonly Factories _factories;
    private readonly RunProgress _runProgress;
    private readonly List<LevelBlockInfo> _levelBlock;
    private readonly LevelBlockInfo _startBlock;

    public Level(
        List<LevelBlockInfo> levelBlocks,
        LevelBlockInfo startBlock,
        Factories factories,
        Transform player,
        RunProgress runProgress) :
        base(levelBlocks, player, true, 0.2f)
    {
        _factories = factories;
        _runProgress = runProgress;
        _levelBlock = levelBlocks;
        _levelBlock.Insert(0, startBlock);
    }

    protected override Dictionary<int, FactoryPoolMono<LevelBlock>> InitializeBlockPools()
    {
        Dictionary<int, FactoryPoolMono<LevelBlock>> blockPools = new();
        for (var i = 0; i < _levelBlock.Count; i++)
        {
            AbstractFactory<LevelBlock> factory = new LevelBlockFactory(
                _levelBlock[i],
                _factories,
                _runProgress);
            blockPools.Add(i, new FactoryPoolMono<LevelBlock>(factory, null, true, 0));
        }

        return blockPools;
    }
}