using UnityEngine;

public sealed class MovingObstacle : Obstacle
{
    private const float Speed = 10;

    private readonly Vector3 _movingDirection = Vector3.back * Speed;

    private Vector3 _startPosition;

    private bool _isEntered;
    
    private RunProgress _runProgress;

    public void Init(Factories factories, bool needShowItems, RunProgress runProgress)
    {
        base.Init(factories, needShowItems);

        _runProgress = runProgress;
        
        _startPosition = transform.localPosition;
    }

    public override void HideObstacle()
    {
        base.HideObstacle();

        transform.localPosition = _startPosition;
        _isEntered = false;
    }

    private void Update()
    {
        if (_isEntered)
        {
            transform.localPosition += _movingDirection * 
                                       (_runProgress.HalfSpeedMultiplier * Time.deltaTime * AsyncUtils.TimeScale);
        }
    }

    public override void Trigger()
    {
        _isEntered = true;
        
        base.Trigger();
    }
}