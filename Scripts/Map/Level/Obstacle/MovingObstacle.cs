using System;
using UnityEngine;

public class MovingObstacle : Obstacle
{
    private const float Speed = 1;

    private readonly Vector3 _movingDirection = Vector3.forward * Speed;

    private Vector3 _startPosition;

    private bool _isEntered;

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
    
    private void Awake()
    {
        _startPosition = transform.localPosition;
    }

    private void Update()
    {
        if (_isEntered)
        {
            transform.localPosition += _movingDirection * Time.deltaTime;
        }
    }
}