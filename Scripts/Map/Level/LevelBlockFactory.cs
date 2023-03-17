using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class LevelBlockFactory: AbstractFactory<LevelBlock>
{
    private const float EmptyFieldSizeZ = 5;
    private const float StartSpawnModificator = 1000;
    private const string EmptyObstacleName = "Empty";
    private const string NullObstacleName = "Null";
    
    private readonly Factories _factories;
    private readonly RunProgress _runProgress;
    private readonly LevelBlockInfo _levelBlockInfo;
    private readonly WeightRandom _weightRandom;
    private readonly Vector3 _startSpawnPoint = Vector3.one * StartSpawnModificator;

    private int _count;

    public LevelBlockFactory(LevelBlockInfo levelBlockInfo, Factories factories, RunProgress runProgress)
    {
        _factories = factories;
        _runProgress = runProgress;
        _levelBlockInfo = levelBlockInfo;
    }

    public override LevelBlock CreateItem()
    {
        GameObject parent = new($"LevelBlock_{_levelBlockInfo.name}_{_count.ToString()}");
        var obstacleBlock = parent.AddComponent<LevelBlock>();
        bool isBlockStatic = CreateBlock(_levelBlockInfo, obstacleBlock);
        _count++;

        if (isBlockStatic)
        {
            StaticBatchingUtility.Combine(parent);
        }

        return obstacleBlock;
    }

    private bool CreateBlock(LevelBlockInfo levelBlockInfo, LevelBlock levelBlock)
    {
        List<(Obstacle obstacle, bool needSpawnItems)> createdObstacles = new();
        float[] spawnPositionsX = Map.AllLinesCoordsX;
        var obstaclesPrefabs = new GameObject[Map.LinesCount];
        var obstaclesNeedSpawnItems = new bool[Map.LinesCount];
        var currentPositionsZ = new float[Map.LinesCount];
        var isBlockStatic = true;
        
        for (int i = levelBlockInfo.Line.Count - 1; i >= 0; i--)
        {
            Line currentLine = levelBlockInfo.Line[i];
            
            obstaclesPrefabs[0] = currentLine.Obstacle1;
            obstaclesPrefabs[1] = currentLine.Obstacle2;
            obstaclesPrefabs[2] = currentLine.Obstacle3;

            obstaclesNeedSpawnItems[0] = currentLine.NeedSpawnItems1;
            obstaclesNeedSpawnItems[1] = currentLine.NeedSpawnItems2;
            obstaclesNeedSpawnItems[2] = currentLine.NeedSpawnItems3;

            for (var j = 0; j < obstaclesPrefabs.Length; j++)
            {
                (Obstacle newObstacle, float positionZ, bool isObstacleStatic) = SpawnObstacle(
                    obstaclesPrefabs[j],
                    obstaclesNeedSpawnItems[j],
                    levelBlock.transform,
                    spawnPositionsX[j],
                    currentPositionsZ[j]);
                currentPositionsZ[j] += positionZ;
                if (newObstacle != null)
                {
                    createdObstacles.Add((newObstacle, obstaclesNeedSpawnItems[j]));
                }

                isBlockStatic = isBlockStatic && isObstacleStatic;
            }
        }
        
        levelBlock.Init(createdObstacles, _factories, _runProgress, currentPositionsZ.Max());

        return isBlockStatic;
    }

    private (Obstacle obstacle, float z, bool isObstacleStatic) SpawnObstacle(
        GameObject obstaclePrefab,
        bool needSpawnItems,
        Transform parent,
        float spawnPosX,
        float spawnPosZ)
    {
        switch (obstaclePrefab ? obstaclePrefab.name : NullObstacleName)
        {
            case NullObstacleName when !needSpawnItems:
                return (null, EmptyFieldSizeZ, true);
            case EmptyObstacleName when !needSpawnItems:
                return (null, 0, true);
            case EmptyObstacleName or NullObstacleName:
                return (InstantiateEmptyObjectWithItems(parent, spawnPosX, spawnPosZ), EmptyFieldSizeZ, true);
        }
        
        GameObject createdObstacle = InstantiateObstacle(
            obstaclePrefab,
            parent,
            spawnPosX,
            spawnPosZ);

        var obstacle = createdObstacle.GetComponent<Obstacle>();

        return (obstacle, GetSizeZ(createdObstacle), obstacle is MovingObstacle);
    }

    private GameObject InstantiateObstacle(GameObject prefab, Transform parent,float spawnPosX, float spawnPosZ)
    {
        GameObject obstacle = Object.Instantiate(prefab, _startSpawnPoint, Quaternion.identity);
        obstacle.transform.parent = parent;
        obstacle.transform.position = new Vector3(spawnPosX, 0, spawnPosZ);

        return obstacle;
    }

    private Obstacle InstantiateEmptyObjectWithItems(Transform parent, float spawnPosX, float spawnPosZ)
    {
        var obstacleObject = new GameObject
        {
            transform =
            {
                parent = parent,
                position = new Vector3(spawnPosX, 1, spawnPosZ + EmptyFieldSizeZ / 2),
            }
        };

        List<ItemParent> obstacleItems = SimpleMoneySpawner.SpawnMoneys(
            obstacleObject.transform,
            new Vector3(0, 0, -EmptyFieldSizeZ / 2),
            EmptyFieldSizeZ);

        var obstacle = obstacleObject.AddComponent<EmptyObstacle>();
        obstacle.InitItems(obstacleItems);

        return obstacle;
    }

    private static float GetSizeZ(GameObject obstacle)
    {
        BoxCollider[] colliders = obstacle.GetComponentsInChildren<BoxCollider>();
        float sizeZ = colliders.Sum(collider => collider.size.z);
        return sizeZ > EmptyFieldSizeZ ? sizeZ : EmptyFieldSizeZ;
    }
}