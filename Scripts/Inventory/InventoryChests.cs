using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryChests : InventoryGrid
{
    //private Chests _chests;
    private bool _isChestOpened = false;

    public InventoryChests(
        InventorySteamworks inventorySteamworks,
        List<GameObject> inventoryCells,
        TMP_Text nameText,
        TMP_Text descriptionText,
        Button button,
        Button nextPageButton,
        Button previousPageButton,
        GameObject chest,
        Sprite invisibleSprite)
        : base(inventorySteamworks, inventoryCells, nameText, descriptionText, button, nextPageButton, previousPageButton, invisibleSprite)
    {
        _inventoryItemsType = InventoryItemType.Chest;
        //_chests = new Chests(chest);
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

            //oundManager.Instance.PlaySound(SoundType.CustomizeInventory, 2);
        });

        //_buttonText.text = LanguageController.CurrentLanguage[MenuInformationManager.ChestsButtonText];
    }

    protected override void AddItemInInventory(List<InventoryItem> addedItems)
    {
        List<int> openedItems = new List<int>();

        if (_isChestOpened)
        {
            for (int i = 0; i < addedItems.Count; i++)
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
        //_chests.PickChest(_items[chestNumber].ID);
        _isChestOpened = true;
    }
}
