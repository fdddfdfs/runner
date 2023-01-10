using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObstacleBlockFactory: AbstractFactory<ObstacleBlock>
{
    private const float EmptyFieldHeight = 5;
    private const string EmptyObstacleName = "Empty";
    
    private readonly Factories _factories;
    private readonly RunProgress _runProgress;
    private readonly LevelBlockInfo _levelBlockInfo;
    private readonly WeightRandom _weightRandom;

    private int _count;

    public ObstacleBlockFactory(LevelBlockInfo levelBlockInfo, Factories factories, RunProgress runProgress)
    {
        _factories = factories;
        _runProgress = runProgress;
        _levelBlockInfo = levelBlockInfo;
    }

    public override ObstacleBlock CreateItem()
    {
        GameObject parent = new($"MapPartParent_{_levelBlockInfo.name}{_count.ToString()}");
        var obstacleBlock = parent.AddComponent<ObstacleBlock>();
        CreateBlock(_levelBlockInfo, obstacleBlock);
        _count++;

        return obstacleBlock;
    }

    private void CreateBlock(LevelBlockInfo levelBlockInfo, ObstacleBlock obstacleBlock)
    {
        List<Obstacle> createdObstacles = new();
        float[] spawnPositionsX = Map.AllLinesCoordsX;
        var obstaclesPrefabs = new GameObject[Map.LinesCount];
        var currentPositionsZ = new float[Map.LinesCount];
        
        for (int i = levelBlockInfo.Line.Count - 1; i >= 0; i--)
        {
            obstaclesPrefabs[0] = levelBlockInfo.Line[i].Obstacle1;
            obstaclesPrefabs[1] = levelBlockInfo.Line[i].Obstacle2;
            obstaclesPrefabs[2] = levelBlockInfo.Line[i].Obstacle3;

            for (var j = 0; j < obstaclesPrefabs.Length; j++)
            {
                (Obstacle newObstacle, float positionZ) = SpawnObstacle(
                    obstaclesPrefabs[j],
                    obstacleBlock.transform,
                    spawnPositionsX[j],
                    currentPositionsZ[j]);
                currentPositionsZ[j] += positionZ;
                if (newObstacle != null)
                {
                    createdObstacles.Add(newObstacle);
                }
            }
        }
        
        obstacleBlock.Init(createdObstacles, _factories, _runProgress, currentPositionsZ.Max());
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