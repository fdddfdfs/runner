using System.Collections.Generic;
using System.Threading;
using StarterAssets;
using UnityEngine;

public sealed class Follower : MonoBehaviour, IRunnable
{
    private const float FollowDelay = 0.3f;

    [SerializeField] private RunProgress _runProgress;
    [SerializeField] private Run _run;
    [SerializeField] private Animator _animator;

    private readonly Queue<Vector3> _targetPositions = new();
    private readonly CancellationToken[] _linkedTokens = new CancellationToken[1];

    private bool _isFollowing;
    private bool _isFollowingDelay;
    private GameObject _target;
    
    private Vector3 _appearStartPosition;
    private Vector3 _appearEndPosition;
    private float _appearTime;
    private float _currentAppearTime;

    private Vector3 _startPosition;

    private CancellationTokenSource _followCancellation;
    private FollowerAnimator _followerAnimator;

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
        SetCancellationToken();
        
        _isFollowing = true;
        _target = target;
        
        SetupAppear();

        Follow((int)((time - FollowDelay) * 1000));
    }

    public void StopFollowing()
    {
        if (_isFollowing)
        {
            _followCancellation.Cancel();
        }
    }

    public void Lose(ThirdPersonController player)
    {
        transform.position = player.transform.position;
        transform.rotation = player.PlayerBones.rotation;
        _followerAnimator.ChangeAnimationTrigger(AnimationType.Lose);
    }

    private void FixedUpdate()
    {
        if (!_isFollowing) return;

        _followerAnimator.ChangeAnimationFloat(AnimationType.Speed ,_runProgress.HalfSpeedMultiplier);
        
        _targetPositions.Enqueue(_target.transform.position);
        
        if (!_isFollowingDelay)
        {
            Vector3 deltaPosition = _targetPositions.Peek() - transform.position;
            _followerAnimator.ChangeAnimationFloat(AnimationType.HorizontalRun, deltaPosition.x);
            transform.position = _targetPositions.Dequeue();
        }
        else
        {
            Appear(Time.fixedDeltaTime * AsyncUtils.TimeScale);
        }
    }

    private void Awake()
    {
        _startPosition = transform.position;
        _followerAnimator = new FollowerAnimator(_animator, transform);
    }

    private void SetCancellationToken()
    {
        if (_followCancellation == null)
        {
            _followCancellation = CancellationTokenSource.CreateLinkedTokenSource(GetLinkedTokens());
            return;
        }
        
        if (_isFollowing || _followCancellation.IsCancellationRequested)
        {
            if (!_followCancellation.IsCancellationRequested)
            {
                _followCancellation.Cancel();
            }

            _followCancellation.Dispose();
            _followCancellation = CancellationTokenSource.CreateLinkedTokenSource(GetLinkedTokens());
        }
    }

    private CancellationToken[] GetLinkedTokens()
    {
        _linkedTokens[0] = _run.GetCancellationToken();
        return _linkedTokens;
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

        CancellationToken token = _followCancellation.Token;
        CancellationToken closeGameToken = AsyncUtils.Instance.GetCancellationToken();
        
        await AsyncUtils.Wait(FollowDelay / _runProgress.SpeedMultiplier, token);
        
        _isFollowingDelay = false;

        if (closeGameToken.IsCancellationRequested) return;
        
        if (token.IsCancellationRequested)
        {
            _isFollowing = false;
            _targetPositions.Clear();
            transform.position = _startPosition;

            return;
        }

        await AsyncUtils.Wait(followTimeMilliseconds, token);
        
        if (closeGameToken.IsCancellationRequested) return;

        if (token.IsCancellationRequested)
        {
            transform.position = _startPosition;
        }

        _isFollowing = false;
        _targetPositions.Clear();
    }
    
    
}