using UnityEngine;

public sealed class EnvironmentMovingAnimatedObject : EnvironmentAnimatedObject
{
    [SerializeField] private Transform _object;
    [SerializeField] private bool _triggeredByTrigger;
    
    private Vector3 _startPosition;
    private Quaternion _startRotation;

    public override void Trigger()
    {
        if (_triggeredByTrigger)
        {
            base.Trigger();
        }
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
        if (!_triggeredByTrigger)
        {
            base.Trigger();
        }
    }

    private void OnDisable()
    {
        Transform objectTransform = _object.transform;
        objectTransform.localPosition = _startPosition;
        objectTransform.localRotation = _startRotation;
    }
}