using System;
using UnityEngine;

public class EnvironmentMovingObject : MonoBehaviour, ITriggerable
{
    [SerializeField] private Transform _object;
    [SerializeField] private Vector3 _direction;

    private Vector3 _startPosition;
    private bool _isActive;
    
    private void FixedUpdate()
    {
        if (!_isActive) return;
        
        _object.localPosition += _direction * AsyncUtils.TimeScale;
    }

    private void Awake()
    {
        if (_object == null)
        {
            _object = transform;
        }
        
        _startPosition = _object.localPosition;
    }

    private void OnEnable()
    {
        _object.localPosition = _startPosition;
    }

    private void OnDisable()
    {
        _isActive = false;
    }

    public void Trigger()
    {
        _isActive = true;
    }
}