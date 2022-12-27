using System.Collections.Generic;
using UnityEngine;

public class ObstacleBlock : MonoBehaviour, IMapBlock
{
    private List<Obstacle> _obstacles;

    public void Init(List<Obstacle> obstacles, Factories factories)
    {
        _obstacles = obstacles;

        foreach (var obstacle in obstacles)
        {
            obstacle.Init(factories);
        }
    }

    public void HideBlock()
    {
        foreach (var obstacle in _obstacles)
        {
            obstacle.HideObstacle();
        }
        
        gameObject.SetActive(false);
    }

    public void EnterBlock()
    {
        foreach (var obstacle in _obstacles)
        {
            obstacle.EnterObstacle();
        }
    }
}