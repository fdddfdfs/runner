using System.Collections.Generic;
using System.Threading;

public class ItemsActiveStates
{
    private readonly ICancellationTokenProvider _cancellationTokenProvider;
    private readonly CancellationToken[] _linkedTokens;
    private readonly Dictionary<ItemType, ActiveState> _itemActiveStates;

    public ItemsActiveStates(ICancellationTokenProvider cancellationTokenProvider)
    {
        _cancellationTokenProvider = cancellationTokenProvider;
        _linkedTokens = new[] { cancellationTokenProvider.GetCancellationToken() };
        _itemActiveStates = new Dictionary<ItemType, ActiveState>();
    }

    public void ActivateItem(ItemType item)
    {
        if (!_itemActiveStates.ContainsKey(item))
        {
            _itemActiveStates.Add(item, new ActiveState(_linkedTokens));
        }

        ActiveState activeState = _itemActiveStates[item];
        if (activeState.IsActive || activeState.CancellationTokenSource.IsCancellationRequested)
        {
            activeState.CancellationTokenSource.Cancel();
            activeState.CancellationTokenSource.Dispose();
            _linkedTokens[0] = _cancellationTokenProvider.GetCancellationToken();
            activeState.CancellationTokenSource = 
                CancellationTokenSource.CreateLinkedTokenSource(_linkedTokens);
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
        
        public ActiveState(CancellationToken[] linkedTokens)
        {
            CancellationTokenSource = 
                CancellationTokenSource.CreateLinkedTokenSource(linkedTokens);
            IsActive = false;
        }
    }
}