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
    private const float MoneyDistance = 5;
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

    private record LevelBlocksPools(GameObjectPool BlockPool, float SizeZ);

    private record SetLevelBlock(GameObject Block, float SizeZ);

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
            
        _firstBlock.Block.SetActive(false);
        _firstBlock = _blocksPositions.Dequeue();
    }

    public void StartRun()
    {
        _isPause = false;

        _lastBlockPosition = SetNewBlocks(0, 0);
        _firstBlock = _blocksPositions.Dequeue();
    }

    public void EndRun()
    {
        while (_blocksPositions.Count != 0)
        {
            _blocksPositions.Dequeue().Block.SetActive(false);
        }
    }
    
    private void Awake()
    {
        _isPause = true;
        
        CalculateBlocksWeights();
    }

    private void Start()
    {
        _itemsFactoryPools = new Dictionary<ItemType, FactoryPool<Item>>();
        foreach (var factories in _factories.ItemFactories.AsEnumerable())
        {
            _itemsFactoryPools[factories.Key] = new FactoryPool<Item>(factories.Value, null, true);
        }

        _blocks = new Dictionary<int, LevelBlocksPools>();
        for (int i = 0; i < _levelBlocks.Count; i++)
        {
            float endPosition = 0;
            List<GameObject> generatedBlocks = new();
            for (int j = 0; j < BaseBlocksGenerationCount; j++)
            {
                GameObject parent = new($"LevelBlockParent_{_levelBlocks[i].name}+{j}");
                endPosition  = GenerateBlock(_levelBlocks[i], parent.transform);
                generatedBlocks.Add(parent);
            }
            _blocks.Add(i, new LevelBlocksPools(new GameObjectPool(generatedBlocks), endPosition));
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
        GameObject block = _blocks[blockId].BlockPool.GetItem();
        block.transform.localPosition = new Vector3(0, BaseYPosition, positionZ);
        for (int i = 0; i < block.transform.childCount; i++)
        {
            block.transform.GetChild(i).gameObject.SetActive(true);
        }

        return new SetLevelBlock(block, _blocks[blockId].SizeZ);
    }

    private float GenerateBlock(LevelBlock pickedBlock, Transform parent)
    {
        float blockEndPosition = 0;

        for (int i = 0; i < pickedBlock.Line.Count; i++)
        {
            float currentPosition = blockEndPosition;
            float lineEndPosition = EmptyFieldHeight;
            float y1, y2, y3;

            (y1, lineEndPosition) = SpawnObstacle(
                pickedBlock.Line[i].Obstacle1,
                parent,
                -ColumnOffset,
                currentPosition,
                lineEndPosition);

            (y2, lineEndPosition) = SpawnObstacle(
                pickedBlock.Line[i].Obstacle2,
                parent,
                0,
                currentPosition,
                lineEndPosition);
            (y3, lineEndPosition) = SpawnObstacle(
                pickedBlock.Line[i].Obstacle3,
                parent,
                ColumnOffset,
                currentPosition,
                lineEndPosition);

            Vector3 leftItemPosition = new Vector3(-ColumnOffset, y1, currentPosition);
            SpawnItemOnBlock(leftItemPosition ,lineEndPosition, parent, pickedBlock.Line[i].ItemType1);

            Vector3 centerItemPosition = new Vector3(0, y2, currentPosition);
            SpawnItemOnBlock(centerItemPosition, lineEndPosition, parent, pickedBlock.Line[i].ItemType2);

            Vector3 rightItemPosition = new Vector3(ColumnOffset, y3, currentPosition);
            SpawnItemOnBlock(rightItemPosition, lineEndPosition, parent, pickedBlock.Line[i].ItemType3);

            blockEndPosition += lineEndPosition;
        }

        return blockEndPosition;
    }

    private void SpawnItemOnBlock(Vector3 position, float blockSizeZ, Transform parent, ItemType itemType)
    {
        if (itemType == ItemType.None)
            return;

        float currentPositionZ = 0;
        while (currentPositionZ < blockSizeZ)
        {
            ItemType type = currentPositionZ + MoneyDistance < blockSizeZ ? ItemType.Money : itemType;

            GameObject item = _itemsFactoryPools[type].GetItem();
            item.transform.parent = parent;
            item.transform.position = position + Vector3.forward * currentPositionZ;
            currentPositionZ += MoneyDistance;
        }
    }

    private (float y, float z) SpawnObstacle(
        GameObject obstaclePrefab,
        Transform parent,
        float spawnPosX,
        float spawnPosZ,
        float endPosition)
    {
        if (obstaclePrefab != null)
        {
            GameObject obstacle = InstantiateObstacle(
                obstaclePrefab,
                parent,
                spawnPosX,
                spawnPosZ);

            return CheckForUpdateEndPosition(obstacle, endPosition);
        }

        return (BaseYPosition / 4, endPosition);
    }

    private GameObject InstantiateObstacle(GameObject prefab, Transform parent,float spawnPosX, float spawnPosZ)
    {
        GameObject obstacle = Instantiate(prefab, parent);
        obstacle.transform.position = new Vector3(spawnPosX, 0, spawnPosZ);

        return obstacle;
    }

    private (float y,float z) CheckForUpdateEndPosition(GameObject obstacle, float currentMaxSize)
    {
        BoxCollider boxCollider = obstacle.GetComponent<BoxCollider>();
        Vector3 size = boxCollider.size;
        
        return (size.y, size.z < currentMaxSize ? currentMaxSize : size.z);
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
