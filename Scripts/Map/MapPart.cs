using System.Collections.Generic;
using UnityEngine;

public abstract class MapPart<TBlockInfo,TBlock> : IRunnable 
    where TBlockInfo: ScriptableObject, IWeightable
    where TBlock: MonoBehaviour, IMapBlock 
{
    private const int DefaultViewDistance = 1000;
    private const float BaseYPosition = 0;
    private const float HideBlockOffset = 5;

    //private Dictionary<int, AbstractFactory<TBlock>> _blocks;
    private Dictionary<int, FactoryPoolMono<TBlock>> _blockPools;

    private readonly Queue<TBlock> _blocksPositions;
    private readonly Queue<TBlock> _blocksToTrigger;
    private readonly WeightRandom _weightRandom;
    private readonly Transform _player;
    private readonly TBlockInfo _startBlock;
    private readonly bool _haveStartBlock;
    private readonly float _nextBlockDistancePercentToHide;
    private readonly float _nextBlockDistanceToTrigger;
    private readonly bool _isTriggerDistancePercents;
    private readonly float _viewDistance;
    
    private TBlock _firstBlock;
    private float _lastBlockPosition;
    private float _firstBlockPosition;
    
    private TBlock _firstTriggerableBlock;
    private float _firstTriggerablePosition;

    private float _hideBlockPosition;
    private float _triggerBlockPosition;

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
        float nextBlockDistanceToTrigger = 0.2f,
        bool isTriggerDistanceInPercents = true,
        float viewDistance = DefaultViewDistance)
    {
        _weightRandom = new WeightRandom(new List<IWeightable>(blocksInfo), true);
        _blocksPositions = new Queue<TBlock>();
        _blocksToTrigger = new Queue<TBlock>();
        _player = player;
        _haveStartBlock = haveStartBlock;
        _nextBlockDistancePercentToHide = nextBlockDistancePercentToHide;
        _nextBlockDistanceToTrigger = nextBlockDistanceToTrigger;
        _isTriggerDistancePercents = isTriggerDistanceInPercents;
        _viewDistance = viewDistance;
    }
    
    public void CheckToHideBlock()
    {
        if (!_firstBlock) return;
            
    
        if (_player.transform.position.z >
            _hideBlockPosition)
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
            UpdateHideBlockPosition();
        }
    }
    
    public void StartRun()
    {
        if (!_haveStartBlock)
        {
            UpdateFirstBlock();
            UpdateHideBlockPosition();
        }

        _firstTriggerableBlock = _blocksToTrigger.Dequeue();
        _triggerBlockPosition = CalculateTriggerBlockPosition();
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

        _firstTriggerableBlock = null;
        _blocksToTrigger.Clear();
        _firstTriggerablePosition = 0f;

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
        if (!_firstTriggerableBlock) return;
        
        if (_triggerBlockPosition < _player.transform.position.z)
        {
            (_firstTriggerableBlock as ITriggerable)?.Trigger();
            UpdateTriggerableBlock();

            return;
        }

        if (!_firstTriggerableBlock.gameObject.activeSelf)
        {
            UpdateTriggerableBlock();
        }
    }

    private void UpdateTriggerableBlock()
    {
        _firstTriggerablePosition += _firstTriggerableBlock.BlockSize;
        _firstTriggerableBlock = _blocksToTrigger.Dequeue();
        _triggerBlockPosition = CalculateTriggerBlockPosition();
    }

    private float CalculateTriggerBlockPosition()
    {
        return _firstTriggerablePosition + 
               (_isTriggerDistancePercents ? 
                   _firstTriggerableBlock.BlockSize * _nextBlockDistanceToTrigger :
                   _nextBlockDistanceToTrigger);
    }

    private void HideCurrentBlock()
    {
        _firstBlock?.HideBlock();

        UpdateFirstBlock();

        _lastBlockPosition = SetNewBlocks(_firstBlockPosition, _lastBlockPosition);

        UpdateHideBlockPosition();
    }

    private void UpdateFirstBlock()
    {
        _firstBlockPosition += _firstBlock?.BlockSize ?? 0;
        _firstBlock = _blocksPositions.Dequeue();
        _firstBlock.EnterBlock();
    }

    private void UpdateHideBlockPosition()
    {
        _hideBlockPosition = _firstBlockPosition +
                             _firstBlock.BlockSize +
                             _blocksPositions.Peek().BlockSize * _nextBlockDistancePercentToHide;
    }

    private float SetNewBlocks(float startPosition, float lastBlockPosition)
    {
        float currentGeneratedDistance = lastBlockPosition;
        float endPosition = _viewDistance + startPosition;
        
        while (currentGeneratedDistance < endPosition)
        {
            int blockID = _weightRandom.GetRandom() + (_haveStartBlock ? 1 : 0);
            TBlock newBlock = SetBlock(blockID, currentGeneratedDistance);
            currentGeneratedDistance += newBlock.BlockSize;
            _blocksPositions.Enqueue(newBlock);
            _blocksToTrigger.Enqueue(newBlock);
        }

        return currentGeneratedDistance;
    }

    private float SetStartBlock()
    {
        TBlock newBlock = SetBlock(0, 0);
        _blocksPositions.Enqueue(newBlock);
        _blocksToTrigger.Enqueue(newBlock);
        return newBlock.BlockSize;
    }
    
    private TBlock SetBlock(int blockId, float positionZ)
    {
        TBlock obstacleBlock = BlockPools[blockId].GetRandomItem();
        obstacleBlock.transform.localPosition = new Vector3(0, BaseYPosition, positionZ);

        return obstacleBlock;
    }
}