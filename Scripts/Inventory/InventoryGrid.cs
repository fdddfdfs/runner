using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class InventoryGrid
{
    protected InventorySteamworks _inventorySteamworks;
    protected List<GameObject> _inventoryCells;
    protected TMP_Text _nameText;
    protected TMP_Text _descriptionText;
    protected Button _button;
    protected TMP_Text _buttonText;
    protected Button _nextPageButton, _previousPageButton;
    protected InventoryItemType _inventoryItemsType;

    protected List<Image> _inventoryCellsImages;
    protected List<Button> _inventoryCellsButtons;
    protected List<TMP_Text> _inventoryCellsStackText;

    protected List<InventoryItem> _items;
    protected List<int> _itemsCount;
    protected List<List<ulong>> _itemsIDs;

    protected List<int> defaultItemsIDs;

    private int _currentPage = 0;
    private int _numberOfPages = 1;

    private Sprite _invisibleSprite;

    public bool IsActive { get; set; }

    public InventoryGrid(
        InventorySteamworks inventorySteamworks,
        List<GameObject> inventoryCells,
        TMP_Text nameText,
        TMP_Text descriptionText,
        Button button,
        Button nextPageButton,
        Button previousPageButton,
        Sprite invisibleSprite)
    {
        _inventorySteamworks = inventorySteamworks;
        _inventoryCells = inventoryCells;
        _nameText = nameText;
        _descriptionText = descriptionText;
        _button = button;
        _nextPageButton = nextPageButton;
        _previousPageButton = previousPageButton;
        _invisibleSprite = invisibleSprite;

        _buttonText = _button.GetComponentInChildren<TMP_Text>();

        _inventoryCellsImages = new List<Image>();
        _inventoryCellsButtons = new List<Button>();
        _inventoryCellsStackText = new List<TMP_Text>();

        for (int i = 0; i < _inventoryCells.Count; i++)
        {
            _inventoryCellsImages.Add(_inventoryCells[i].GetComponent<Image>());
            _inventoryCellsButtons.Add(_inventoryCells[i].GetComponent<Button>());
            _inventoryCellsButtons[^1].enabled = false;
            _inventoryCellsStackText.Add(_inventoryCells[i].GetComponentInChildren<TMP_Text>());

            _inventoryCellsStackText[^1].text = string.Empty;
        }

        _inventorySteamworks.InventoryLoaded += InitializeInventory;
        _inventorySteamworks.InventoryAddItem += AddItemInInventory;
        _inventorySteamworks.InventoryRemoveItem += RemoveItemFromInventory;


        if (!SteamManager.Initialized)
        {
            InitializeInventory(new List<InventoryItem>());
        }
    }

    public void ClearEvents()
    {
        _inventorySteamworks.InventoryLoaded -= InitializeInventory;
        _inventorySteamworks.InventoryAddItem -= AddItemInInventory;
        _inventorySteamworks.InventoryRemoveItem -= RemoveItemFromInventory;
    }

    protected void InitializeInventory(List<InventoryItem> inventoryItems)
    {
        _items = new List<InventoryItem>();
        _itemsCount = new List<int>();
        _itemsIDs = new List<List<ulong>>();

        AddDefaultItems();

        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].InventoryItemData.InventoryItemType != _inventoryItemsType)
            {
                continue;
            }

            _items.Add(inventoryItems[i]);
            _itemsCount.Add(1);
            _itemsIDs.Add(new List<ulong>() { inventoryItems[i].SteamID });
        }

        StackItems();

        _numberOfPages = Mathf.CeilToInt((float)_items.Count /_inventoryCells.Count);
    }

    protected void RemoveItemFromInventory(List<InventoryItem> removedItems)
    {
        bool isActiveInStart = IsActive;

        if (isActiveInStart)
        {
            ClearItems();
        }

        for (int i = 0; i < removedItems.Count; i++)
        {
            if (removedItems[i].InventoryItemData.InventoryItemType != _inventoryItemsType)
                continue;

            int removedItemIndex = _items.FindIndex(
                item => item.InventoryItemData.ID == removedItems[i].InventoryItemData.ID);

            if (_itemsCount[removedItemIndex] == 1)
            {
                _items.RemoveAt(removedItemIndex);
                _itemsCount.RemoveAt(removedItemIndex);
                _itemsIDs.RemoveAt(removedItemIndex);
            }
            else
            {
                _itemsCount[removedItemIndex] -= 1;

                for (int j = 0; j < _itemsIDs[removedItemIndex].Count; j++)
                {
                    if (_itemsIDs[removedItemIndex][j] == removedItems[i].SteamID)
                    {
                        _itemsIDs[removedItemIndex].RemoveAt(j);
                        break;
                    }
                }
            }
        }

        _numberOfPages = Mathf.CeilToInt((float)_items.Count / _inventoryCells.Count);

        if (isActiveInStart)
            ShowItems();
    }

    protected virtual void AddItemInInventory(List<InventoryItem> addedItems)
    {
        bool isActiveInStart = IsActive;

        if (isActiveInStart)
            ClearItems();

        for (int i = 0; i < addedItems.Count; i++)
        {
            if (addedItems[i].InventoryItemData.InventoryItemType != _inventoryItemsType)
                continue;

            int addedItemIndex = _items.FindIndex(
                item => item.InventoryItemData.ID == addedItems[i].InventoryItemData.ID);

            if (addedItemIndex == -1)
            {
                _items.Add(addedItems[i]);
                _itemsCount.Add(1);
            }
            else
            {
                _itemsCount[addedItemIndex] += 1;
                _itemsIDs[addedItemIndex].Add(addedItems[i].SteamID);
            }
        }

        _numberOfPages = Mathf.CeilToInt((float)_items.Count / _inventoryCells.Count);

        if (isActiveInStart)
            ShowItems();
    }

    public void ShowItems()
    {
        int numberOfItems = Mathf.Min(_items.Count - _currentPage * _inventoryCells.Count, _inventoryCells.Count);

        for (var i = 0; i < numberOfItems; i++)
        {
            IsActive = true;
            _inventoryCellsImages[i].sprite = _items[i + _currentPage * _inventoryCells.Count].InventoryItemData.Icon;

            int itemNumber = i + _currentPage * _inventoryCells.Count;
            _inventoryCellsButtons[i].enabled = true;
            _inventoryCellsButtons[i].onClick.AddListener(() => ShowItemInfo(itemNumber));

            int count = _itemsCount[i + _currentPage * _inventoryCells.Count];
            _inventoryCellsStackText[i].text = $"x{count.ToString()}";
        }

        _nextPageButton.onClick.AddListener(() => ChangePage(1));
        _previousPageButton.onClick.AddListener(() => ChangePage(-1));

        ChangePageButtonsVisible();
        if (_items.Count != 0)
        {
            ShowItemInfo(0);
        }
    }

    public void ClearItems()
    {
        int numberOfItems = Mathf.Min(_items.Count - _currentPage * _inventoryCells.Count, _inventoryCells.Count);

        for (int i = 0; i < numberOfItems; i++)
        {
            IsActive = false;
            if (_inventoryCellsImages[i] != null)
            {
                _inventoryCellsImages[i].sprite = _invisibleSprite;
            }

            _inventoryCellsButtons[i].onClick.RemoveAllListeners();
            _inventoryCellsButtons[i].enabled = false;

            if (_inventoryCellsStackText[i] != null)
            {
                _inventoryCellsStackText[i].text = string.Empty;
            }
        }

        _nextPageButton.onClick.RemoveAllListeners();
        _previousPageButton.onClick.RemoveAllListeners();
    }

    private void StackItems()
    {
        for (int i = 0; i < _items.Count; i++)
        {
            List<InventoryItem> sameItems = _items.FindAll(
                item => item.InventoryItemData.ID == _items[i].InventoryItemData.ID);

            _itemsCount[i] = sameItems.Count;

            _itemsIDs.Add(new List<ulong>() { _items[i].SteamID });

            for (int j = i + 1; j < _items.Count; j++)
            {
                if(_items[j].InventoryItemData.ID == _items[i].InventoryItemData.ID)
                {
                    _itemsIDs[i].Add(_items[j].SteamID);

                    _items.RemoveAt(j);
                    _itemsCount.RemoveAt(j);
                    _itemsIDs.RemoveAt(j);
                    j--;
                }
            }
        }
    }

    protected virtual void ShowItemInfo(int chestNumber)
    {
        _nameText.text = Localization.Instance[_items[chestNumber].InventoryItemData.Name];
        _descriptionText.text = Localization.Instance[_items[chestNumber].InventoryItemData.Description];
    }

    private void ChangePage(int dir)
    {
        ClearItems();

        _currentPage += dir;
        
        ShowItems();
    }

    private void ChangePageButtonsVisible()
    {
        if (_currentPage == 0)
        {
            _previousPageButton.gameObject.SetActive(false);
            _previousPageButton.enabled = false;
        }
        else if (!_previousPageButton.gameObject.activeSelf)
        {
            _previousPageButton.gameObject.SetActive(true);
            _previousPageButton.enabled = true;
        }

        if (_currentPage >= _numberOfPages - 1)
        {
            _nextPageButton.gameObject.SetActive(false);
            _nextPageButton.enabled = false;
        }
        else if (!_nextPageButton.gameObject.activeSelf)
        {
            _nextPageButton.gameObject.SetActive(true);
            _nextPageButton.enabled = true;
        }
    }

    protected virtual void AddDefaultItems()
    {
        if (defaultItemsIDs == null)
            return;

        for (var i = 0; i < defaultItemsIDs.Count; i++)
        {
            _items.Add(new InventoryItem(0, InventoryAllItems.Instance.Items[defaultItemsIDs[i]]));
            _itemsCount.Add(1);
            _itemsIDs.Add(new List<ulong>() { 0 });
        }
    }
}
