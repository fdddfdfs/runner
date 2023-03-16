using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public sealed class ActiveItemsUI : MonoBehaviour, IRunnable
{
    [SerializeField] private List<ItemData> _itemsData;
    [SerializeField] private Transform _parent;
    [SerializeField] private GameObject _prefab;

    private PoolMono<ActiveItem> _activeItems;
    private Dictionary<ItemType, ActiveItem> _currentlyActiveItems;
    private Dictionary<ItemType, ItemData> _items;

    public void ShowNewItemEffect(ItemType itemType, float time)
    {
        ActiveItem item = _currentlyActiveItems.ContainsKey(itemType) 
            ? _currentlyActiveItems[itemType] 
            : _activeItems.GetItem();
        
        item.ItemImage.sprite = _items[itemType].Icon;
        foreach (Image progressImage in item.ProgressImages)
        {
            progressImage.DOKill();
            progressImage.fillAmount = 1;
            progressImage.DOFillAmount(0, time).SetEase(Ease.Linear).OnComplete(() =>
            {
                item.gameObject.SetActive(false);
                _currentlyActiveItems.Remove(itemType);
            });
        }

        _currentlyActiveItems[itemType] = item;
    }

    public void HideEffect(ItemType itemType)
    {
        if (!_currentlyActiveItems.ContainsKey(itemType))
        {
            return;
        }
        
        ActiveItem item = _currentlyActiveItems[itemType];

        foreach (var progressImage in item.ProgressImages)
        {
            progressImage.DOKill();
        }

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
    }
}
