using System.Collections.Generic;
using UnityEngine;

public abstract class MapPart<TBlockInfo,TBlock> : IRunnable 
    where TBlockInfo: ScriptableObject, IWeightable
    where TBlock: MonoBehaviour, IMapBlock 
{
    private const int ViewDistance = 400;
    private const float BaseYPosition = 0;
    private const float HideBlockOffset = 5;

    private Dictionary<int, AbstractFactory<TBlock>> _blocks;
    private Dictionary<int, FactoryPoolMono<TBlock>> _blockPools;

    private readonly Queue<TBlock> _blocksPositions;
    private readonly WeightRandom _weightRandom;
    private readonly Transform _player;

    private TBlock _firstBlock;
    private float _lastBlockPosition;
    private float _firstBlockPosition;

    private Dictionary<int, FactoryPoolMono<TBlock>> BlockPools
    {
        get
        {
            _blockPools ??= InitializeBlockPools();
            return _blockPools;
        }
    }

    protected MapPart(
        List<TBlockInfo> blocksInfo,
        Transform player)
    {
        // TODO: change it to false when make more SO
        _weightRandom = new WeightRandom(new List<IWeightable>(blocksInfo), true);
        _blocksPositions = new Queue<TBlock>();
        _player = player;
    }
    
    public void CheckToHideBlock()
    {
        if (_player.transform.position.z > _firstBlockPosition + _firstBlock.BlockSize)
        {
            HideCurrentBlock();
        }
    }
    
    public void StartRun()
    {
        _firstBlockPosition = 0;
        _lastBlockPosition = SetNewBlocks(0, 0);
        UpdateFirstBlock();
    }

    public void EndRun()
    {
        _firstBlock?.HideBlock();

        _firstBlock = null;
        
        while (_blocksPositions.Count != 0)
        {
            _blocksPositions.Dequeue().HideBlock();
        }
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

    private void HideCurrentBlock()
    {
        _firstBlock?.HideBlock();

        UpdateFirstBlock();
        
        _lastBlockPosition = SetNewBlocks(_firstBlockPosition, _lastBlockPosition);
    }

    private void UpdateFirstBlock()
    {
        _firstBlockPosition += _firstBlock?.BlockSize ?? 0;
        _firstBlock = _blocksPositions.Dequeue();
        _firstBlock.EnterBlock();
    }

    private float SetNewBlocks(float startPosition, float lastBlockPosition)
    {
        float currentGeneratedDistance = lastBlockPosition;
        float endPosition = ViewDistance + startPosition;
        
        while (currentGeneratedDistance < endPosition)
        {
            int blockID = _weightRandom.GetRandom();
            TBlock newBlock = SetBlock(blockID, currentGeneratedDistance);
            currentGeneratedDistance += newBlock.BlockSize;
            _blocksPositions.Enqueue(newBlock);
        }

        return currentGeneratedDistance;
    }
    
    private TBlock SetBlock(int blockId, float positionZ)
    {
        TBlock obstacleBlock = BlockPools[blockId].GetItem();
        obstacleBlock.transform.localPosition = new Vector3(0, BaseYPosition, positionZ);

        return obstacleBlock;
    }
}