﻿using System.Collections.Generic;
using UnityEngine;

public abstract class MapPart<TBlockInfo,TBlock> 
    where TBlockInfo: ScriptableObject, IWeightable
    where TBlock: MonoBehaviour, IMapBlock
{
    private const int BaseBlocksGenerationCount = 20;
    private const int ViewDistance = 1000;
    private const float BaseYPosition = 1;
    private const float HideBlockOffset = 5;

    private Dictionary<int, BlocksPools> _blocks;

    private readonly Queue<SettedBlock> _blocksPositions;
    private readonly WeightRandom _weightRandom;
    private readonly List<TBlockInfo> _blocksInfo;
    private readonly Transform _player;
    private readonly int _blocksGenerationCount;
    
    private SettedBlock _firstBlock;
    private float _lastBlockPosition;
    
    private record BlocksPools(PoolMono<TBlock> BlockPool, float SizeZ);
    
    private record SettedBlock(TBlock ObstacleBlock, float SizeZ);

    private Dictionary<int, BlocksPools> Blocks
    {
        get
        {
            if (_blocks != null) return _blocks;
            InitializeBlocks();
            return _blocks;

        }
        set => _blocks = value;
    }

    protected MapPart(
        List<TBlockInfo> blocksInfo,
        Transform player,
        int blocksGenerationCount = BaseBlocksGenerationCount)
    {
        // TODO: change it to false when make more SO
        _weightRandom = new WeightRandom(new List<IWeightable>(blocksInfo), true);
        _blocksPositions = new Queue<SettedBlock>();
        _blocksInfo = blocksInfo;
        _player = player;
        _blocksGenerationCount = blocksGenerationCount;
    }
    
    public void CheckToHideBlock()
    {
        if (_player.transform.position.z > _firstBlock.ObstacleBlock.transform.position.z + _firstBlock.SizeZ)
        {
            HideCurrentBlock();
        }
    }
    
    public void StartRun()
    {
        _lastBlockPosition = SetNewBlocks(0, 0);
        UpdateFirstBlock();
    }

    public void EndRun()
    {
        while (_blocksPositions.Count != 0)
        {
            _blocksPositions.Dequeue().ObstacleBlock.gameObject.SetActive(false);
        }
    }
    
    public void HideCurrentBlock()
    {
        if (_player.transform.position.z + HideBlockOffset < _firstBlock.ObstacleBlock.transform.position.z)
        {
            return;
        }
        
        _lastBlockPosition = SetNewBlocks(_firstBlock.ObstacleBlock.transform.position.z, _lastBlockPosition);
        
        _firstBlock.ObstacleBlock.HideBlock();
        UpdateFirstBlock();
    }

    protected abstract float GenerateBlock(TBlockInfo block, TBlock parent);

    private void UpdateFirstBlock()
    {
        _firstBlock = _blocksPositions.Dequeue();
        _firstBlock.ObstacleBlock.EnterBlock();
    }

    private void InitializeBlocks()
    {
        Blocks = new Dictionary<int, BlocksPools>();
        for (int i = 0; i < _blocksInfo.Count; i++)
        {
            float endPosition = 0;
            List<TBlock> generatedBlocks = new();
            for (int j = 0; j < _blocksGenerationCount; j++)
            {
                GameObject parent = new($"MapPartParent_{_blocksInfo[i].name}+{j}");
                TBlock obstacleBlock = parent.AddComponent<TBlock>();
                endPosition = GenerateBlock(_blocksInfo[i], obstacleBlock);
                generatedBlocks.Add(obstacleBlock);
            }
            Blocks.Add(i, new BlocksPools(new PoolMono<TBlock>(generatedBlocks), endPosition));
        }
    }

    private float SetNewBlocks(float startPosition, float lastBlockPosition)
    {
        float currentGeneratedDistance = lastBlockPosition;
        float endPosition = ViewDistance + startPosition;

        while (currentGeneratedDistance < endPosition)
        {
            int blockID = _weightRandom.GetRandom();
            SettedBlock newBlock = SetBlock(blockID, currentGeneratedDistance);
            currentGeneratedDistance += newBlock.SizeZ;
            _blocksPositions.Enqueue(newBlock);
        }

        return currentGeneratedDistance;
    }
    
    private SettedBlock SetBlock(int blockId, float positionZ)
    {
        TBlock obstacleBlock = Blocks[blockId].BlockPool.GetItem();
        obstacleBlock.transform.localPosition = new Vector3(0, BaseYPosition, positionZ);

        return new SettedBlock(obstacleBlock, Blocks[blockId].SizeZ);
    }
}