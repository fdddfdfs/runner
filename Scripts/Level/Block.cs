using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
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

    public void HideObstacle()
    {
        foreach (var obstacle in _obstacles)
        {
            obstacle.HideObstacle();
        }
    }
}