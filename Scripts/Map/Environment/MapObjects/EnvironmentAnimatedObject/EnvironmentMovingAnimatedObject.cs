using UnityEngine;

public sealed class EnvironmentMovingAnimatedObject : EnvironmentAnimatedObject
{
    [SerializeField] private Transform _object;
    
    private Vector3 _startPosition;
    private Quaternion _startRotation;
    
    public override void Trigger()
    {
        
    }
    
    private void Start()
    {
        if (_object == null)
        {
            _object = transform;
        }
        
        Transform objectTransform = _object.transform;
        _startPosition = objectTransform.localPosition;
        _startRotation = objectTransform.localRotation;
    }

    private void OnEnable()
    {
        base.Trigger();
    }

    private void OnDisable()
    {
        Transform objectTransform = _object.transform;
        objectTransform.localPosition = _startPosition;
        objectTransform.localRotation = _startRotation;
    }
}