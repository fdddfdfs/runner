using System.Collections.Generic;
using System.Threading;

public class ItemsActiveStates
{
    private readonly Run _run;
    private readonly Dictionary<ItemType, ActiveState> _itemActiveStates;

    public ItemsActiveStates(Run run)
    {
        _run = run;
        _itemActiveStates = new Dictionary<ItemType, ActiveState>();
    }

    public void ActivateItem(ItemType item)
    {
        if (!_itemActiveStates.ContainsKey(item))
        {
            _itemActiveStates.Add(item, new ActiveState(_run));
        }

        ActiveState activeState = _itemActiveStates[item];
        if (activeState.IsActive)
        {
            activeState.CancellationTokenSource.Cancel();
            activeState.CancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(_run.EndRunToken);
        }

        activeState.IsActive = true;
    }

    public void DeactivateItem(ItemType item)
    {
        _itemActiveStates[item].IsActive = false;
    }

    public CancellationToken GetTokenForItem(ItemType item)
    {
        return _itemActiveStates[item].CancellationTokenSource.Token;
    }
    
    private class ActiveState
    {
        public bool IsActive;
        public CancellationTokenSource CancellationTokenSource;
        
        public ActiveState(Run run)
        {
            CancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(run.EndRunToken);
            IsActive = false;
        }
    }
}