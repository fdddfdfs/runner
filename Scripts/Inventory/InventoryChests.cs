using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class InventoryChests : InventoryGrid
{
    private bool _isChestOpened;

    public InventoryChests(
        InventorySteamworks inventorySteamworks,
        List<GameObject> inventoryCells,
        TMP_Text nameText,
        TMP_Text descriptionText,
        Button button,
        Button nextPageButton,
        Button previousPageButton,
        Sprite invisibleSprite)
        : base(inventorySteamworks,
            inventoryCells,
            nameText,
            descriptionText,
            button,
            nextPageButton,
            previousPageButton,
            invisibleSprite)
    {
        _inventoryItemsType = InventoryItemType.Chest;
    }

    protected override void ShowItemInfo(int chestNumber)
    {
        base.ShowItemInfo(chestNumber);

        if (!_button.gameObject.activeSelf)
        {
            _button.gameObject.SetActive(true);
        }

        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(() =>
        {
            OpenChest(chestNumber);
        });

        _buttonText.text = Localization.Instance[AllTexts.OpenChest];
    }

    protected override void AddItemInInventory(List<InventoryItem> addedItems)
    {
        List<int> openedItems = new ();

        if (_isChestOpened)
        {
            for (var i = 0; i < addedItems.Count; i++)
            {
                if (addedItems[i].InventoryItemData.InventoryItemType != _inventoryItemsType)
                {
                    openedItems.Add(addedItems[i].InventoryItemData.ID);
                }
            }

            _isChestOpened = false;
        }

        if (openedItems.Count != 0)
        {
            //_chests.ShowOpenedItems(openedItems);
        }

        base.AddItemInInventory(addedItems);
    }

    private void OpenChest(int chestNumber)
    {
        _button.gameObject.SetActive(false);
        _inventorySteamworks.OpenChest(_itemsIDs[chestNumber][^1]);
        _isChestOpened = true;
    }
}
