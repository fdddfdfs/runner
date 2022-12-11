using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ActiveItemsUI : MonoBehaviour
{
    [SerializeField] private List<ItemType> _itemTypes;
    [SerializeField] private List<Sprite> _itemSprites;
    [SerializeField] private Transform _parent;
    [SerializeField] private GameObject _prefab;
    
    private PoolMono<ActiveItem> _activeItems;
    private Dictionary<ItemType, Sprite> _items;
    private Dictionary<ItemType, ActiveItem> _currentlyActiveItems;

    private void Awake()
    {
        _currentlyActiveItems = new Dictionary<ItemType, ActiveItem>();
        
        _items = new Dictionary<ItemType, Sprite>();
        for (int i = 0; i < _itemTypes.Count; i++)
        {
            _items.Add(_itemTypes[i], _itemSprites[i]);
        }

        _activeItems = new PoolMono<ActiveItem>(_prefab, _parent, true, 5);
    }

    public void ShowNewItemEffect(ItemType itemType, float time)
    {
        ActiveItem item = _currentlyActiveItems.ContainsKey(itemType) 
            ? _currentlyActiveItems[itemType] 
            : _activeItems.GetItem();

        item.ItemImage.sprite = _items[itemType];
        item.ProgressImage.DOKill();
        item.ProgressImage.fillAmount = 1;
        item.ProgressImage.DOFillAmount(0, time).SetEase(Ease.Linear).OnComplete(() =>
            {
                item.gameObject.SetActive(false);
                _currentlyActiveItems.Remove(itemType);
            });
        
        _currentlyActiveItems[itemType] = item;
    }

    public void HideEffect(ItemType itemType)
    {
        if (!_currentlyActiveItems.ContainsKey(itemType))
        {
            return;
        }
        
        ActiveItem item = _currentlyActiveItems[itemType];

        item.ProgressImage.DOKill();
        item.gameObject.SetActive(false);
        _currentlyActiveItems.Remove(itemType);
    }
}
