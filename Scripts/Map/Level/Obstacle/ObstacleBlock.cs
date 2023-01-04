using System.Collections.Generic;
using UnityEngine;

public class ObstacleBlock : MonoBehaviour, IMapBlock
{
    private List<Obstacle> _obstacles;

    public void Init(List<Obstacle> obstacles, Factories factories, RunProgress runProgress)
    {
        _obstacles = obstacles;

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