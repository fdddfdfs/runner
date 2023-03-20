using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class LevelBlock : MonoBehaviour, IMapBlock, ITriggerable
{
    private List<Obstacle> _obstacles;
    private bool _needShowItems;

    private List<ITriggerable> _triggerables;

    public float BlockSize { get; private set; }

    public void Init(
        List<(Obstacle obstacle,bool needSpawnItems)> obstacles,
        Factories factories,
        RunProgress runProgress,
        float blockSize)
    {
        _obstacles = obstacles.Select(tuple => tuple.obstacle).ToList();
        BlockSize = blockSize;
        _triggerables = new List<ITriggerable>();
        
        foreach ((Obstacle obstacle,bool needSpawnItems) obstacle in obstacles)
        {
            if (obstacle.obstacle is MovingObstacle movingObstacle)
            {
                movingObstacle.Init(factories, obstacle.needSpawnItems, runProgress);
            }
            else
            {
                obstacle.obstacle.Init(factories, obstacle.needSpawnItems);
            }
            
            _triggerables.Add(obstacle.obstacle);
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

    public void Trigger()
    {
        foreach (ITriggerable triggerable in _triggerables)
        {
            triggerable.Trigger();
        }
    }
}