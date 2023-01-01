using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class Follower : MonoBehaviour
{
    private const float FollowDelay = 0.1f;

    [SerializeField] private RunProgress _runProgress;
    
    private readonly Queue<Vector3> _targetPositions = new();

    private bool _isFollowing;
    private bool _isFollowingDelay;
    private GameObject _target;
    private WaitForSeconds _followWaiter;
    private float _previousTime = -1;

    private Vector3 _appearStartPosition;
    private Vector3 _appearEndPosition;
    private float _appearTime;
    private float _currentAppearTime;

    public void FollowForTime(GameObject target, float time)
    {
        _isFollowing = true;
        _target = target;
        
        SetupAppear();

        if (_previousTime < 0 || Math.Abs(_previousTime - time) > 0.01f)
        {
            _followWaiter = new WaitForSeconds(time - FollowDelay);
            _previousTime = time;
        }

        StartCoroutine(Follow());
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
        _appearStartPosition = position - Vector3.back * 5;
        _appearEndPosition = position;
        _appearTime = FollowDelay / _runProgress.SpeedMultiplayer;
        _currentAppearTime = 0;
    }

    private IEnumerator Follow()
    {
        _isFollowing = true;
        _isFollowingDelay = true;

        yield return new WaitForSeconds(FollowDelay / _runProgress.SpeedMultiplayer);

        _isFollowingDelay = false;

        yield return _followWaiter;

        _isFollowing = false;
        _targetPositions.Clear();
    }
}