using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class Follower : MonoBehaviour
{
    private const float FollowDelay = 0.1f;

    private readonly WaitForSeconds _followDelayWaiter = new(FollowDelay);
    private readonly Queue<Vector3> _targetPositions = new();

    private bool _isFollowing;
    private bool _isFollowingDelay;
    private GameObject _target;
    private WaitForSeconds _followWaiter;
    private float _previousTime = -1;

    public void FollowForTime(GameObject target, float time)
    {
        _isFollowing = true;
        _target = target;

        if (_previousTime < 0 || Math.Abs(_previousTime - time) > 0.01f)
        {
            _followWaiter = new WaitForSeconds(time - FollowDelay);
            _previousTime = time;
        }

        StartCoroutine(Follow());
    }

    private void FixedUpdate()
    {
        if (_isFollowing)
        {
            _targetPositions.Enqueue(_target.transform.position);

            if (!_isFollowingDelay)
            {
                transform.position = _targetPositions.Dequeue();
            }
        }
    }

    private IEnumerator Follow()
    {
        _isFollowing = true;
        _isFollowingDelay = true;

        yield return _followDelayWaiter;

        _isFollowingDelay = false;

        yield return _followWaiter;

        _isFollowing = false;
        _targetPositions.Clear();
    }
}