using UnityEngine;

public class EnvironmentMovingAnimatedObject : EnvironmentAnimatedObject
{
    [SerializeField] private Transform _object;
    
    private Vector3 _startPosition;
    private Quaternion _startRotation;
    
    private void Start()
    {
        if (_object == null)
        {
            _object = transform;
        }
        
        Transform objectTransform = _object.transform;
        _startPosition = objectTransform.position;
        _startRotation = objectTransform.rotation;
    }

    private void OnDisable()
    {
        Transform objectTransform = _object.transform;
        objectTransform.position = _startPosition;
        objectTransform.rotation = _startRotation;
    }
}