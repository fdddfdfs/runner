public readonly struct InventoryItem
{
    public ulong SteamID { get; }
    public InventoryItemData InventoryItemData { get; }

    public InventoryItem(ulong steamID, InventoryItemData inventoryItemData)
    {
        InventoryItemData = inventoryItemData;
        SteamID = steamID;
    }
}
