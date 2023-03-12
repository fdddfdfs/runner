using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using System;

public class InventorySteamworks
{
    private List<SteamItemDetails_t> _inventoryItems;

    private Callback<SteamInventoryResultReady_t> _inventoryItemsResult;

    public Action<List<InventoryItem>> OnInventoryLoaded;
    public Action<List<InventoryItem>> OnInventoryAddItem;
    public Action<List<InventoryItem>> OnInventoryRemoveItem;

    private SteamInventoryResult_t initializeResult;
    private SteamInventoryResult_t updateResult;

    public InventorySteamworks()
    {
        if (!SteamManager.Initialized)
        {
            return;
        }

        _inventoryItems = new List<SteamItemDetails_t>();

        _inventoryItemsResult = Callback<SteamInventoryResultReady_t>.Create(OnGetInventoryItems);

        SteamInventory.GetAllItems(out initializeResult);
    }

    public void Unregister()
    {
        _inventoryItemsResult.Unregister();
    }


    private void OnGetInventoryItems(SteamInventoryResultReady_t resultReady)
    {
        if (resultReady.m_result != EResult.k_EResultOK)
        {
            throw new System.Exception("Failed getting inventory");
        }

        uint length = 0;
        bool getItemsResult = SteamInventory.GetResultItems(resultReady.m_handle, null, ref length);

        SteamItemDetails_t[] changedItemsDetails = new SteamItemDetails_t[length];
        getItemsResult = SteamInventory.GetResultItems(resultReady.m_handle, changedItemsDetails, ref length);

        List<InventoryItem> changedItems = new List<InventoryItem>();
        for (int i = 0; i < changedItemsDetails.Length; i++)
        {
            if (changedItemsDetails[i].m_iDefinition.m_SteamItemDef==0)
            {
                SteamInventory.DestroyResult(updateResult);
                return;
            }

            InventoryItem item = InventoryAllItems.AllItems[changedItemsDetails[i].m_iDefinition.m_SteamItemDef];
            item.SteamID = changedItemsDetails[i].m_itemId.m_SteamItemInstanceID;
            changedItems.Add(item);
        }

        if (initializeResult.m_SteamInventoryResult != -100) //Начальная инициализация инвентаря
        {
            for (int i = 0; i < changedItemsDetails.Length; i++)
            {
                _inventoryItems.Add(changedItemsDetails[i]);
            }

            OnInventoryLoaded?.Invoke(changedItems);

            SteamInventory.DestroyResult(initializeResult);
            initializeResult.m_SteamInventoryResult = -100;

            CheckForPromoItems();
        }
        else if(changedItems.Count!=0) //Удаление или добавление нового предмета в инвентарь
        {
            List<InventoryItem> removedItems = new List<InventoryItem>();
            List<InventoryItem> addedItems = new List<InventoryItem>();

            for (int i = 0; i < changedItems.Count; i++)
            {
                if (changedItemsDetails[i].m_unQuantity == 0)
                {
                    _inventoryItems.Remove(changedItemsDetails[i]);

                    removedItems.Add(changedItems[i]);
                }
                else
                {
                    _inventoryItems.Add(changedItemsDetails[i]);

                    addedItems.Add(changedItems[i]);
                }
            }

            if (addedItems.Count != 0)
                OnInventoryAddItem?.Invoke(addedItems);

            if (removedItems.Count != 0)
                OnInventoryRemoveItem?.Invoke(removedItems);

            SteamInventory.DestroyResult(updateResult);
        }
    }

    public void OpenChest(ulong chestSteamID)
    {
        SteamItemDetails_t chest = FindItemBySteamID(chestSteamID);
        SteamItemInstanceID_t[] chests = new SteamItemInstanceID_t[] { chest.m_itemId };
        SteamItemDef_t[] generatedItems = new SteamItemDef_t[] { new SteamItemDef_t(chest.m_iDefinition.m_SteamItemDef * 10) };
        uint[] itemsQuantity = new uint[] { 1 };
        SteamInventory.ExchangeItems(out updateResult, generatedItems, itemsQuantity, 1, chests, itemsQuantity, 1);
    }

    private SteamItemDetails_t FindItemBySteamID(ulong steamID)
    {
        foreach (var item in _inventoryItems)
        {
            if (item.m_itemId.m_SteamItemInstanceID == steamID)
            {
                return item;
            }
        }

        return default;
    }

    private void CheckForPromoItems()
    {
        SteamInventory.GrantPromoItems(out updateResult);
    }
}
