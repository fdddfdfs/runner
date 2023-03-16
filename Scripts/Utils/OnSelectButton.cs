using System;
using UnityEngine;
using UnityEngine.EventSystems;

public sealed class OnSelectButton : MonoBehaviour, ISelectHandler
{
    private Action _onSelectAction;
    
    public void Init(Action onSelectAction)
    {
        _onSelectAction = onSelectAction;
    }
    
    public void OnSelect(BaseEventData eventData)
    {
        _onSelectAction?.Invoke();
    }
}