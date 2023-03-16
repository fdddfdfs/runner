using System.Collections.Generic;
using Steamworks;
using System;

public sealed class InventorySteamworks
{
    private readonly List<SteamItemDetails_t> _inventoryItems;
    private readonly Callback<SteamInventoryResultReady_t> _inventoryItemsResult;
    private readonly uint[] _openChestItemsQuantity = { 1 };
    private readonly SteamItemInstanceID_t[] _openedChest = new SteamItemInstanceID_t[1];
    private readonly SteamItemDef_t[] _openedItems = new SteamItemDef_t[1];

    public Action<List<InventoryItem>> InventoryLoaded;
    public Action<List<InventoryItem>> InventoryAddItem;
    public Action<List<InventoryItem>> InventoryRemoveItem;

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

            InventoryLoaded?.Invoke(changedItems);

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
                InventoryAddItem?.Invoke(addedItems);

            if (removedItems.Count != 0)
                InventoryRemoveItem?.Invoke(removedItems);
        }
        
        SteamInventory.DestroyResult(_steamInventoryResult);
    }

    public void OpenChest(ulong chestSteamID)
    {
        SteamItemDetails_t chest = FindItemBySteamID(chestSteamID);
        _openedChest[0] = chest.m_itemId;
        _openedItems[0] = new SteamItemDef_t( chest.m_iDefinition.m_SteamItemDef * 10);

        SteamInventory.ExchangeItems(
            out _steamInventoryResult,
            _openedItems,
            _openChestItemsQuantity,
            (uint)_openChestItemsQuantity.Length,
            _openedChest,
            _openChestItemsQuantity,
            (uint)_openChestItemsQuantity.Length);
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
