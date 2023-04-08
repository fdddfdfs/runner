using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public sealed class ActiveItemsUI : MonoBehaviour, IRunnable
{
    [SerializeField] private List<ItemData> _itemsData;
    [SerializeField] private Transform _parent;
    [SerializeField] private GameObject _prefab;

    private readonly CancellationToken[] _linkedTokens = new CancellationToken[1];
    
    private PoolMono<ActiveItem> _activeItems;
    private Dictionary<ItemType, ActiveItem> _currentlyActiveItems;
    private Dictionary<ItemType, ItemData> _items;

    private Dictionary<ItemType, CancellationTokenSource> _cancellationTokenSources;

    public void ShowNewItemEffect(ItemType itemType, float time)
    {
        ActiveItem item;

        if (_currentlyActiveItems.ContainsKey(itemType))
        {
            item = _currentlyActiveItems[itemType];
            
            _cancellationTokenSources[itemType].Cancel();
        }
        else
        {
            item = _activeItems.GetItem();
            item.ItemImage.sprite = _items[itemType].Icon;
            _currentlyActiveItems[itemType] = item;

            item.Text.text = itemType == ItemType.Board ? $"x{Stats.BoardCount.Value.ToString()}" : string.Empty;
        }
        
        CheckCancellationTokenSource(itemType);
        
        ShowActiveEffect(item, itemType, time);
    }

    private void CheckCancellationTokenSource(ItemType itemType)
    {
        CancellationTokenSource itemTokenSource = _cancellationTokenSources.GetValueOrDefault(itemType, null);
        if (itemTokenSource == null || itemTokenSource.IsCancellationRequested)
        {
            itemTokenSource?.Dispose();
            _linkedTokens[0] = AsyncUtils.Instance.GetCancellationToken();
            _cancellationTokenSources[itemType] = CancellationTokenSource.CreateLinkedTokenSource(_linkedTokens);
        }
    }

    private async void ShowActiveEffect(ActiveItem item, ItemType itemType, float time)
    {
        float currentTime = 0;

        CancellationToken token = _cancellationTokenSources[itemType].Token;
        
        foreach (Image progressImage in item.ProgressImages)
        {
            progressImage.fillAmount = 1;
        }

        while (currentTime < time)
        {
            await Task.Yield();
            if (AsyncUtils.TimeScale == 0) continue;

            foreach (Image progressImage in item.ProgressImages)
            {
                progressImage.fillAmount = 1 - currentTime / time;
            }
            
            currentTime += Time.deltaTime * AsyncUtils.TimeScale;
            
            if (token.IsCancellationRequested) return;
        }
        
        HideEffect(itemType, false);
    }

    public void HideEffect(ItemType itemType, bool isForceCancel = true)
    {
        if (!_currentlyActiveItems.ContainsKey(itemType))
        {
            return;
        }
        
        if (isForceCancel)
        {
            _cancellationTokenSources[itemType].Cancel();
        }
        
        ActiveItem item = _currentlyActiveItems[itemType];

        item.gameObject.SetActive(false);
        _currentlyActiveItems.Remove(itemType);
    }

    public void StartRun() { }

    public void EndRun()
    {
        foreach (ItemType itemType in _items.Keys)
        {
            HideEffect(itemType);
        }
    }
    
    private void Awake()
    {
        _currentlyActiveItems = new Dictionary<ItemType, ActiveItem>();

        _items = new Dictionary<ItemType, ItemData>();
        foreach (ItemData itemData in _itemsData)
        {
            _items[itemData.Type] = itemData;
        }

        _activeItems = new GameObjectPoolMono<ActiveItem>(_prefab, _parent, true, 5);

        _cancellationTokenSources = new Dictionary<ItemType, CancellationTokenSource>();
    }
}
