using System.Collections.Generic;
using Steamworks;
using System;

public class InventorySteamworks
{
    private readonly List<SteamItemDetails_t> _inventoryItems;
    private readonly Callback<SteamInventoryResultReady_t> _inventoryItemsResult;

    public Action<List<InventoryItem>> OnInventoryLoaded;
    public Action<List<InventoryItem>> OnInventoryAddItem;
    public Action<List<InventoryItem>> OnInventoryRemoveItem;

    private SteamInventoryResult_t _steamInventoryResult;

    private bool _initialized;

    public InventorySteamworks()
    {
        if (!SteamManager.Initialized)
        {
            return;
        }

        _inventoryItems = new List<SteamItemDetails_t>();

        _inventoryItemsResult = Callback<SteamInventoryResultReady_t>.Create(OnGetInventoryItems);

        SteamInventory.GetAllItems(out _steamInventoryResult);
    }

    public void SetInventoryResult(SteamInventoryResult_t inventoryResult)
    {
        _steamInventoryResult = inventoryResult;
    }

    public void Unregister()
    {
        _inventoryItemsResult.Unregister();
    }


    private void OnGetInventoryItems(SteamInventoryResultReady_t resultReady)
    {
        if (resultReady.m_result != EResult.k_EResultOK)
        {
            throw new Exception("Failed getting inventory");
        }

        uint length = 0;
        bool getItemsResult = SteamInventory.GetResultItems(resultReady.m_handle, null, ref length);

        if (!getItemsResult)
        {
            throw new Exception("Failed getting items length");
        }

        var changedItemsDetails = new SteamItemDetails_t[length];
        getItemsResult = SteamInventory.GetResultItems(resultReady.m_handle, changedItemsDetails, ref length);

        if (!getItemsResult)
        {
            throw new Exception("Failed getting items");
        }

        List<InventoryItem> changedItems = new();
        for (var i = 0; i < changedItemsDetails.Length; i++)
        {
            if (changedItemsDetails[i].m_iDefinition.m_SteamItemDef == 0)
            {
                SteamInventory.DestroyResult(_steamInventoryResult);
                return;
            }

            InventoryItemData data = 
                InventoryAllItems.Instance.Items[changedItemsDetails[i].m_iDefinition.m_SteamItemDef];
            ulong steamID = changedItemsDetails[i].m_itemId.m_SteamItemInstanceID;

            var item = new InventoryItem(steamID, data);
            changedItems.Add(item);
        }

        if (!_initialized)
        {
            for (var i = 0; i < changedItemsDetails.Length; i++)
            {
                _inventoryItems.Add(changedItemsDetails[i]);
            }

            OnInventoryLoaded?.Invoke(changedItems);

            CheckForPromoItems();

            _initialized = true;
        }
        else if(changedItems.Count != 0)
        {
            List<InventoryItem> removedItems = new ();
            List<InventoryItem> addedItems = new ();

            for (var i = 0; i < changedItems.Count; i++)
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
        }
        
        SteamInventory.DestroyResult(_steamInventoryResult);
    }

    public void OpenChest(ulong chestSteamID)
    {
        SteamItemDetails_t chest = FindItemBySteamID(chestSteamID);
        SteamItemInstanceID_t[] chests = { chest.m_itemId };
        SteamItemDef_t[] generatedItems = { new (chest.m_iDefinition.m_SteamItemDef * 10) };
        uint[] itemsQuantity = { 1 };
        SteamInventory.ExchangeItems(
            out _steamInventoryResult,
            generatedItems,
            itemsQuantity,
            1,
            chests,
            itemsQuantity,
            1);
    }

    private SteamItemDetails_t FindItemBySteamID(ulong steamID)
    {
        foreach (SteamItemDetails_t item in _inventoryItems)
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
        SteamInventory.GrantPromoItems(out _steamInventoryResult);
    }
}
