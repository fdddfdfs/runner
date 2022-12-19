using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class Level : MonoBehaviour, IPauseable
{
    public const int ColumnOffset = 3;
    
    private const float EmptyFieldHeight = 10;
    private const int ViewDistance = 1000;
    private const float BaseYPosition = 1;
    private const int BaseBlocksGenerationCount = 20;
    private const float HideBlockOffset = 5;

    [SerializeField] private List<LevelBlock> _levelBlocks;
    [SerializeField] private Transform _player;
    [SerializeField] private Factories _factories;

    private Dictionary<int, LevelBlocksPools> _blocks;
    private Queue<SetLevelBlock> _blocksPositions;
    private SetLevelBlock _firstBlock;
    private float _lastBlockPosition;
    private int[] _weightsPoints;

    private Dictionary<ItemType, FactoryPool<Item>> _itemsFactoryPools;

    private bool _isPause;

    private record LevelBlocksPools(PoolMono<Block> BlockPool, float SizeZ);

    private record SetLevelBlock(Block Block, float SizeZ);

    public static float GetClosestColumn(float positionX)
    {
        return positionX < -ColumnOffset / 2f ? -ColumnOffset : positionX < ColumnOffset / 2f ? 0 : ColumnOffset;
    }

    public void Pause()
    {
        _isPause = true;
    }

    public void UnPause()
    {
        _isPause = false;
    }
    
    public void HideCurrentBlock()
    {
        if (_player.transform.position.z + HideBlockOffset < _firstBlock.Block.transform.position.z)
        {
            return;
        }
        
        _lastBlockPosition = SetNewBlocks(_firstBlock.Block.transform.position.z, _lastBlockPosition);
        
        _firstBlock.Block.HideBlock();
        UpdateFirstBlock();
    }

    public void StartRun()
    {
        _isPause = false;

        _lastBlockPosition = SetNewBlocks(0, 0);
        UpdateFirstBlock();
    }

    public void EndRun()
    {
        while (_blocksPositions.Count != 0)
        {
            _blocksPositions.Dequeue().Block.gameObject.SetActive(false);
        }
    }
    
    private void Awake()
    {
        _isPause = true;
        
        CalculateBlocksWeights();
    }

    private void Start()
    {
        _blocks = new Dictionary<int, LevelBlocksPools>();
        for (int i = 0; i < _levelBlocks.Count; i++)
        {
            float endPosition = 0;
            List<Block> generatedBlocks = new();
            for (int j = 0; j < BaseBlocksGenerationCount; j++)
            {
                List<Obstacle> createdObstacles;
                GameObject parent = new($"LevelBlockParent_{_levelBlocks[i].name}+{j}");
                Block block = parent.AddComponent<Block>();
                (endPosition, createdObstacles) = GenerateBlock(_levelBlocks[i], parent.transform);
                block.Init(createdObstacles, _factories);
                generatedBlocks.Add(block);
            }
            _blocks.Add(i, new LevelBlocksPools(new PoolMono<Block>(generatedBlocks), endPosition));
        }

        _blocksPositions = new Queue<SetLevelBlock>();
    }

    private void Update()
    {
        if (_isPause)
            return;
        
        if (_player.transform.position.z > _firstBlock.Block.transform.position.z + _firstBlock.SizeZ)
        {
            HideCurrentBlock();
        }
    }

    private void UpdateFirstBlock()
    {
        _firstBlock = _blocksPositions.Dequeue();
        _firstBlock.Block.EnterBlock();
    }

    private float SetNewBlocks(float startPosition, float lastBlockPosition)
    {
        float currentGeneratedDistance = lastBlockPosition;
        float endPosition = ViewDistance + startPosition;

        while (currentGeneratedDistance < endPosition)
        {
            int blockID = GetBlockID();
            SetLevelBlock newBlock = SetBlock(blockID, currentGeneratedDistance);
            currentGeneratedDistance += newBlock.SizeZ;
            _blocksPositions.Enqueue(newBlock);
        }

        return currentGeneratedDistance;
    }

    private SetLevelBlock SetBlock(int blockId, float positionZ)
    {
        Block block = _blocks[blockId].BlockPool.GetItem();
        block.transform.localPosition = new Vector3(0, BaseYPosition, positionZ);

        return new SetLevelBlock(block, _blocks[blockId].SizeZ);
    }

    private (float, List<Obstacle>) GenerateBlock(LevelBlock pickedBlock, Transform parent)
    {
        float blockEndPosition = 0;
        List<Obstacle> createdObstacles = new();

        for (int i = 0; i < pickedBlock.Line.Count; i++)
        {
            float currentPosition = blockEndPosition;
            float lineEndPosition = EmptyFieldHeight;
            Obstacle newObstacle;

            (newObstacle, lineEndPosition) = SpawnObstacle(
                pickedBlock.Line[i].Obstacle1,
                parent,
                -ColumnOffset,
                currentPosition,
                lineEndPosition);
            if (newObstacle != null)
            {
                createdObstacles.Add(newObstacle);
            }

            (newObstacle, lineEndPosition) = SpawnObstacle(
                pickedBlock.Line[i].Obstacle2,
                parent,
                0,
                currentPosition,
                lineEndPosition);
            if (newObstacle != null)
            {
                createdObstacles.Add(newObstacle);
            }
            
            (newObstacle, lineEndPosition) = SpawnObstacle(
                pickedBlock.Line[i].Obstacle3,
                parent,
                ColumnOffset,
                currentPosition,
                lineEndPosition);
            if (newObstacle != null)
            {
                createdObstacles.Add(newObstacle);
            }
            
            blockEndPosition += lineEndPosition;
        }

        return (blockEndPosition, createdObstacles);
    }

    private (Obstacle obstacle, float z) SpawnObstacle(
        GameObject obstaclePrefab,
        Transform parent,
        float spawnPosX,
        float spawnPosZ,
        float endPosition)
    {
        if (obstaclePrefab != null)
        {
            GameObject createdObstacle = InstantiateObstacle(
                obstaclePrefab,
                parent,
                spawnPosX,
                spawnPosZ);

            Obstacle obstacle = createdObstacle.GetComponent<Obstacle>();

            return (obstacle, CheckForUpdateEndPosition(createdObstacle, endPosition));
        }

        return (null, endPosition);
    }

    private GameObject InstantiateObstacle(GameObject prefab, Transform parent,float spawnPosX, float spawnPosZ)
    {
        GameObject obstacle = Instantiate(prefab, parent);
        obstacle.transform.position = new Vector3(spawnPosX, 0, spawnPosZ);

        return obstacle;
    }

    private float CheckForUpdateEndPosition(GameObject obstacle, float currentMaxSize)
    {
        BoxCollider boxCollider = obstacle.GetComponent<BoxCollider>();
        Vector3 size = boxCollider.size;
        
        return size.z < currentMaxSize ? currentMaxSize : size.z;
    }

    private int GetBlockID()
    {
        int r = Random.Range(0, _weightsPoints[^1]);
        int blockID = Array.BinarySearch(_weightsPoints, r);

        return blockID < 0 ? ~blockID : blockID;
    }

    private void CalculateBlocksWeights()
    {
        _weightsPoints = new int[_levelBlocks.Count];
        int currentTotalWeight = 0;
        for (int i = 0; i < _levelBlocks.Count; i++)
        {
            currentTotalWeight += _levelBlocks[i].Weight;
            _weightsPoints[i] = currentTotalWeight;
        }
    }
}
