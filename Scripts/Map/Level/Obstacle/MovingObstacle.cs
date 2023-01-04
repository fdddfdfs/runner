using System;
using UnityEngine;

public class MovingObstacle : Obstacle
{
    private const float Speed = 15;

    private readonly Vector3 _movingDirection = Vector3.back * Speed;

    private Vector3 _startPosition;

    private bool _isEntered;
    
    private RunProgress _runProgress;

    public void Init(Factories factories, RunProgress runProgress)
    {
        base.Init(factories);

        _runProgress = runProgress;
        
        _startPosition = transform.localPosition;
    }

    public override void HideObstacle()
    {
        base.HideObstacle();

        transform.localPosition = _startPosition;
        _isEntered = false;
    }

    public override void EnterObstacle()
    {
        base.EnterObstacle();
        
        _isEntered = true;
    }

    private void Update()
    {
        if (_isEntered)
        {
            transform.localPosition += _movingDirection * (_runProgress.SpeedMultiplier * Time.deltaTime);
        }
    }
}