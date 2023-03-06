using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public sealed class ActiveItemsUI : MonoBehaviour, IRunnable
{
    [SerializeField] private List<ItemType> _itemTypes;
    [SerializeField] private List<Sprite> _itemSprites;
    [SerializeField] private Transform _parent;
    [SerializeField] private GameObject _prefab;
    
    private PoolMono<ActiveItem> _activeItems;
    private Dictionary<ItemType, Sprite> _items;
    private Dictionary<ItemType, ActiveItem> _currentlyActiveItems;

    public void ShowNewItemEffect(ItemType itemType, float time)
    {
        ActiveItem item = _currentlyActiveItems.ContainsKey(itemType) 
            ? _currentlyActiveItems[itemType] 
            : _activeItems.GetItem();
        
        item.ItemImage.sprite = _items[itemType];
        foreach (var progressImage in item.ProgressImages)
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
        foreach (ItemType itemType in _itemTypes)
        {
            HideEffect(itemType);
        }
    }
    
    private void Awake()
    {
        _currentlyActiveItems = new Dictionary<ItemType, ActiveItem>();
        
        _items = new Dictionary<ItemType, Sprite>();
        for (var i = 0; i < _itemTypes.Count; i++)
        {
            _items.Add(_itemTypes[i], _itemSprites[i]);
        }

        _activeItems = new GameObjectPoolMono<ActiveItem>(_prefab, _parent, true, 5);
    }
}
