using System.Collections.Generic;
using UnityEngine;

public class ObstacleBlock : MonoBehaviour, IMapBlock
{
    private List<Obstacle> _obstacles;
    private float _blockSize;

    public float BlockSize => _blockSize;

    public void Init(List<Obstacle> obstacles, Factories factories, RunProgress runProgress, float blockSize)
    {
        _obstacles = obstacles;
        _blockSize = blockSize;
        
        foreach (Obstacle obstacle in obstacles)
        {
            if (obstacle is MovingObstacle movingObstacle)
            {
                movingObstacle.Init(factories, runProgress);
            }
            else
            {
                obstacle.Init(factories);
            }
        }
    }

    public void HideBlock()
    {
        foreach (Obstacle obstacle in _obstacles)
        {
            obstacle.HideObstacle();
        }
        
        gameObject.SetActive(false);
    }

    public void EnterBlock()
    {
        foreach (Obstacle obstacle in _obstacles)
        {
            obstacle.EnterObstacle();
        }
    }
}