using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapPart<TBlockInfo,TBlock> : IRunnable 
    where TBlockInfo: ScriptableObject, IWeightable
    where TBlock: MonoBehaviour, IMapBlock 
{
    private const int ViewDistance = 750;
    private const float BaseYPosition = 0;
    private const float HideBlockOffset = 5;

    private Dictionary<int, AbstractFactory<TBlock>> _blocks;
    private Dictionary<int, FactoryPoolMono<TBlock>> _blockPools;

    private readonly Queue<TBlock> _blocksPositions;
    private readonly WeightRandom _weightRandom;
    private readonly Transform _player;
    private readonly TBlockInfo _startBlock;
    private readonly bool _haveStartBlock;
    private readonly float _nextBlockDistancePercentToHide;
    private readonly float _nextBlockDistancePercentToTrigger;
    
    private TBlock _firstBlock;
    private float _lastBlockPosition;
    private float _firstBlockPosition;
    private ITriggerable _nextTriggerableBlock;

    private Dictionary<int, FactoryPoolMono<TBlock>> BlockPools
    {
        get
        {
            _blockPools ??= InitializeBlockPools();
            return _blockPools;
        }
    }

    protected MapPart(
        IEnumerable<TBlockInfo> blocksInfo,
        Transform player,
        bool haveStartBlock = false,
        float nextBlockDistancePercentToHide = 0,
        float nextBlockDistancePercentToTrigger = 0.2f)
    {
        // TODO: change it to false when make more SO
        _weightRandom = new WeightRandom(new List<IWeightable>(blocksInfo), true);
        _blocksPositions = new Queue<TBlock>();
        _player = player;
        _haveStartBlock = haveStartBlock;
        _nextBlockDistancePercentToHide = nextBlockDistancePercentToHide;
        _nextBlockDistancePercentToTrigger = nextBlockDistancePercentToTrigger;
    }
    
    public void CheckToHideBlock()
    {
        if (!_firstBlock) return;
            
    
        if (_player.transform.position.z >
            _firstBlockPosition + 
            _firstBlock.BlockSize +
            _blocksPositions.Peek().BlockSize * _nextBlockDistancePercentToHide)
        {
            HideCurrentBlock();
        }

        CheckToTriggerBlock();
    }

    public void SetBlocks()
    {
        _firstBlockPosition = 0;
        if (_haveStartBlock)
        {
            _lastBlockPosition = SetStartBlock();
            UpdateFirstBlock();
        }
        
        _lastBlockPosition = SetNewBlocks(_firstBlockPosition, _lastBlockPosition);
        
        if (_haveStartBlock)
        {
            _nextTriggerableBlock = _blocksPositions.Peek() as ITriggerable;
        }
    }
    
    public void StartRun()
    {
        if (!_haveStartBlock)
        {
            UpdateFirstBlock();
        }
    }

    public void EndRun()
    {
        _firstBlock?.HideBlock();

        _firstBlock = null;
        
        while (_blocksPositions.Count != 0)
        {
            _blocksPositions.Dequeue().HideBlock();
        }

        _lastBlockPosition = 0;

        SetBlocks();
    }

    public void HideCurrentEnteredBlock()
    {
        if (_player.transform.position.z + HideBlockOffset < _firstBlockPosition)
        {
            return;
        }
        
        HideCurrentBlock();
    }
    
    public void HideBlocksBeforePositionZ(float positionZ)
    {
        while (_firstBlockPosition < positionZ)
        {
            HideCurrentBlock();
        }
    }
    
    protected abstract Dictionary<int, FactoryPoolMono<TBlock>> InitializeBlockPools();

    private void CheckToTriggerBlock()
    {
        if (_nextTriggerableBlock == null) return;
        
        if(_firstBlock.BlockSize * (1 - _nextBlockDistancePercentToTrigger) < _player.transform.position.z)
        {
            _nextTriggerableBlock.Trigger();
            _nextTriggerableBlock = null;
        }
    }

    private void HideCurrentBlock()
    {
        _firstBlock?.HideBlock();
        
        _nextTriggerableBlock?.Trigger();

        UpdateFirstBlock();
        
        _lastBlockPosition = SetNewBlocks(_firstBlockPosition, _lastBlockPosition);
    }

    private void UpdateFirstBlock()
    {
        _firstBlockPosition += _firstBlock?.BlockSize ?? 0;
        _firstBlock = _blocksPositions.Dequeue();
        _firstBlock.EnterBlock();

        if (_blocksPositions.Count != 0)
        {
            _nextTriggerableBlock = _blocksPositions.Peek() as ITriggerable;
        }
    }

    private float SetNewBlocks(float startPosition, float lastBlockPosition)
    {
        float currentGeneratedDistance = lastBlockPosition;
        float endPosition = ViewDistance + startPosition;
        
        while (currentGeneratedDistance < endPosition)
        {
            int blockID = _weightRandom.GetRandom() + (_haveStartBlock ? 1 : 0);
            TBlock newBlock = SetBlock(blockID, currentGeneratedDistance);
            currentGeneratedDistance += newBlock.BlockSize;
            _blocksPositions.Enqueue(newBlock);
        }

        return currentGeneratedDistance;
    }

    private float SetStartBlock()
    {
        TBlock newBlock = SetBlock(0, 0);
        _blocksPositions.Enqueue(newBlock);
        return newBlock.BlockSize;
    }
    
    private TBlock SetBlock(int blockId, float positionZ)
    {
        TBlock obstacleBlock = BlockPools[blockId].GetRandomItem();
        obstacleBlock.transform.localPosition = new Vector3(0, BaseYPosition, positionZ);

        return obstacleBlock;
    }
}