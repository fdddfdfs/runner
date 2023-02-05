using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public sealed class Follower : MonoBehaviour, IRunnable
{
    private const float FollowDelay = 0.2f;
    private const int FollowDelayMilliseconds = (int)(FollowDelay * 1000);

    [SerializeField] private RunProgress _runProgress;
    [SerializeField] private Run _run;
    
    private readonly Queue<Vector3> _targetPositions = new();

    private bool _isFollowing;
    private bool _isFollowingDelay;
    private GameObject _target;
    
    private Vector3 _appearStartPosition;
    private Vector3 _appearEndPosition;
    private float _appearTime;
    private float _currentAppearTime;

    private Vector3 _startPosition;

    public void StartRun()
    {
        gameObject.SetActive(true);
        transform.position = _startPosition;
    }

    public void EndRun()
    {
        _isFollowing = false;
        _isFollowingDelay = false;
        
        gameObject.SetActive(false);
    }
    
    public void FollowForTime(GameObject target, float time)
    {
        _isFollowing = true;
        _target = target;
        
        SetupAppear();

        Follow((int)((time - FollowDelay) * 1000));
    }

    private void FixedUpdate()
    {
        if (!_isFollowing) return;
        
        _targetPositions.Enqueue(_target.transform.position);

        if (!_isFollowingDelay)
        {
            transform.position = _targetPositions.Dequeue();
        }
        else
        {
            Appear(Time.fixedDeltaTime);
        }
    }

    private void Awake()
    {
        _startPosition = transform.position;
    }

    private void Appear(float deltaTime)
    {
        transform.position = Vector3.Lerp(
            _appearStartPosition,
            _appearEndPosition,
            _currentAppearTime / _appearTime);
        _currentAppearTime += deltaTime;
    }

    private void SetupAppear()
    {
        var position = _target.transform.position;
        _appearStartPosition = position + Vector3.back * 5;
        _appearEndPosition = position;
        _appearTime = FollowDelay / _runProgress.SpeedMultiplier;
        _currentAppearTime = 0;
    }

    private async void Follow(int followTimeMilliseconds)
    {
        _isFollowing = true;
        _isFollowingDelay = true;

        CancellationToken token = _run.EndRunToken;
        
        await Task.Delay((int)(FollowDelayMilliseconds / _runProgress.SpeedMultiplier), token)
            .ContinueWith(GlobalCancellationToken.EmptyTask);

        _isFollowingDelay = false;
        
        if (token.IsCancellationRequested)
        {
            _isFollowing = false;
            _targetPositions.Clear();
            return;
        }

        await Task.Delay(followTimeMilliseconds, token).ContinueWith(GlobalCancellationToken.EmptyTask);

        _isFollowing = false;
        _targetPositions.Clear();
    }
    
    
}