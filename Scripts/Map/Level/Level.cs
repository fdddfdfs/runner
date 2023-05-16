﻿using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public sealed class Level : MapPart<LevelBlockInfo, LevelBlock>
{
    private const float HideLevelDistancePercent = 0.2f;
    private const float TriggerLevelDistance = -20f;
    
    private readonly Factories _factories;
    private readonly RunProgress _runProgress;
    private readonly List<LevelBlockInfo> _levelBlock;
    private readonly LevelBlockInfo _startBlock;
    private readonly ThirdPersonController _player;

    private bool _isStartBlock;

    public Level(
        List<LevelBlockInfo> levelBlocks,
        LevelBlockInfo startBlock,
        Factories factories,
        ThirdPersonController player,
        RunProgress runProgress) :
        base(
            levelBlocks,
            player.transform,
            true,
            HideLevelDistancePercent,
            TriggerLevelDistance,
            false)
    {
        _factories = factories;
        _runProgress = runProgress;
        _levelBlock = levelBlocks;
        _levelBlock.Insert(0, startBlock);
        _player = player;
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

    public override void CheckToHideBlock()
    {
        if (_isStartBlock && 
            (_firstBlockPosition + _firstBlock.BlockSize) * (1 - HideLevelDistancePercent) 
            < _player.transform.position.z)
        {
            _player.ChangeHorizontalMoveRestriction(
                _player.HorizontalMoveRestrictions[typeof(HorizontalMoveRestriction)]);

            _isStartBlock = false;
        }
        
        base.CheckToHideBlock();
    }

    protected override void StartRunSetup()
    {
        _isStartBlock = true;
        
        base.StartRunSetup();
    }
}