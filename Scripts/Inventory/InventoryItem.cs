public readonly struct InventoryItem
{
    public ulong SteamID { get; init; }
    public InventoryItemData InventoryItemData { get; init; }

    public InventoryItem(ulong steamID, InventoryItemData inventoryItemData)
    {
        InventoryItemData = inventoryItemData;
        SteamID = steamID;
    }
}
