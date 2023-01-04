using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

public sealed class Level : MapPart<LevelBlockInfo, ObstacleBlock>
{
    private const float EmptyFieldHeight = 5;
    private const string EmptyObstacleName = "Empty";

    private readonly Factories _factories;
    private readonly RunProgress _runProgress;

    private WeightRandom _weightRandom;

    public Level(
        List<LevelBlockInfo> levelBlocks,
        Factories factories,
        Transform player,
        RunProgress runProgress) :
        base(levelBlocks, player)
    {
        _factories = factories;
        _runProgress = runProgress;
    }

    protected override float GenerateBlock(LevelBlockInfo pickedBlockInfo, ObstacleBlock parent)
    {
        List<Obstacle> createdObstacles = new();
        float[] spawnPositionsX = Map.AllLinesCoordsX;
        var obstaclesPrefabs = new GameObject[Map.LinesCount];
        var currentPositionsZ = new float[Map.LinesCount];
        
        for (int i = pickedBlockInfo.Line.Count - 1; i >= 0; i--)
        {
            obstaclesPrefabs[0] = pickedBlockInfo.Line[i].Obstacle1;
            obstaclesPrefabs[1] = pickedBlockInfo.Line[i].Obstacle2;
            obstaclesPrefabs[2] = pickedBlockInfo.Line[i].Obstacle3;

            for (var j = 0; j < obstaclesPrefabs.Length; j++)
            {
                (Obstacle newObstacle, float positionZ) = SpawnObstacle(
                    obstaclesPrefabs[j],
                    parent.transform,
                    spawnPositionsX[j],
                    currentPositionsZ[j]);
                currentPositionsZ[j] += positionZ;
                if (newObstacle != null)
                {
                    createdObstacles.Add(newObstacle);
                }
            }
        }
        
        parent.Init(createdObstacles, _factories, _runProgress);
        
        return currentPositionsZ.Max();
    }

    private static (Obstacle obstacle, float z) SpawnObstacle(
        GameObject obstaclePrefab,
        Transform parent,
        float spawnPosX,
        float spawnPosZ)
    {
        if (obstaclePrefab != null)
        {
            if (obstaclePrefab.name == EmptyObstacleName)
            {
                return (null, 0);
            }
            
            GameObject createdObstacle = InstantiateObstacle(
                obstaclePrefab,
                parent,
                spawnPosX,
                spawnPosZ);

            Obstacle obstacle = createdObstacle.GetComponent<Obstacle>();

            return (obstacle, GetSizeZ(createdObstacle));
        }

        return (null, EmptyFieldHeight);
    }

    private static GameObject InstantiateObstacle(GameObject prefab, Transform parent,float spawnPosX, float spawnPosZ)
    {
        GameObject obstacle = Object.Instantiate(prefab, parent);
        obstacle.transform.position = new Vector3(spawnPosX, 0, spawnPosZ);

        return obstacle;
    }

    private static float GetSizeZ(GameObject obstacle)
    {
        float sizeZ = obstacle.GetComponent<BoxCollider>().size.z;
        return sizeZ > EmptyFieldHeight ? sizeZ : EmptyFieldHeight;
    }
}