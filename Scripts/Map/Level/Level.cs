using System.Collections.Generic;
using UnityEngine;

public sealed class Level : MapPart<LevelBlockInfo, ObstacleBlock>
{
    private const float EmptyFieldHeight = 10;

    private readonly Factories _factories;
    private readonly float _columnOffset;
    private readonly RunProgress _runProgress;

    private WeightRandom _weightRandom;

    public Level(
        List<LevelBlockInfo> levelBlocks,
        Factories factories,
        Transform player,
        float columnOffset,
        RunProgress runProgress) :
        base(levelBlocks, player)
    {
        _columnOffset = columnOffset;
        _factories = factories;
        _runProgress = runProgress;
    }

    protected override float GenerateBlock(LevelBlockInfo pickedBlockInfo, ObstacleBlock parent)
    {
        float blockEndPosition = 0;
        List<Obstacle> createdObstacles = new();

        for (int i = pickedBlockInfo.Line.Count - 1; i >= 0; i--)
        {
            float currentPosition = blockEndPosition;
            float lineEndPosition = EmptyFieldHeight;
            Obstacle newObstacle;

            (newObstacle, lineEndPosition) = SpawnObstacle(
                pickedBlockInfo.Line[i].Obstacle1,
                parent.transform,
                -_columnOffset,
                currentPosition,
                lineEndPosition);
            if (newObstacle != null)
            {
                createdObstacles.Add(newObstacle);
            }

            (newObstacle, lineEndPosition) = SpawnObstacle(
                pickedBlockInfo.Line[i].Obstacle2,
                parent.transform,
                0,
                currentPosition,
                lineEndPosition);
            if (newObstacle != null)
            {
                createdObstacles.Add(newObstacle);
            }
            
            (newObstacle, lineEndPosition) = SpawnObstacle(
                pickedBlockInfo.Line[i].Obstacle3,
                parent.transform,
                _columnOffset,
                currentPosition,
                lineEndPosition);
            if (newObstacle != null)
            {
                createdObstacles.Add(newObstacle);
            }
            
            blockEndPosition += lineEndPosition;
        }
        
        parent.Init(createdObstacles, _factories, _runProgress);
        return blockEndPosition;
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
        GameObject obstacle = Object.Instantiate(prefab, parent);
        obstacle.transform.position = new Vector3(spawnPosX, 0, spawnPosZ);

        return obstacle;
    }

    private float CheckForUpdateEndPosition(GameObject obstacle, float currentMaxSize)
    {
        BoxCollider boxCollider = obstacle.GetComponent<BoxCollider>();
        Vector3 size = boxCollider.size;
        
        return size.z < currentMaxSize ? currentMaxSize : size.z;
    }
}