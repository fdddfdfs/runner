using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Triggerable : MonoBehaviour, ITriggerable
{
    [SerializeField] private List<GameObject> _triggerableObjects;

    private List<ITriggerable> _triggerables;

    protected void Init()
    {
        _triggerables = new List<ITriggerable>();
        for (var i = 0; i < _triggerableObjects.Count; i++)
        {
            var triggerable = _triggerableObjects[i].GetComponent<ITriggerable>();
            if (triggerable == null)
            {
                throw new Exception("Triggerable object don`t have ITriggerable component");
            }
            
            _triggerables.Add(triggerable);
        }
    }
    
    public void Trigger()
    {
        foreach (var triggerable in _triggerables)
        {
            triggerable.Trigger();
        }
    }
}