using UnityEngine;

public abstract class InventoryItemData : ScriptableObject
{
    [SerializeField] private int _id;
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private Sprite _icon;

    public int ID => _id;
    public string Name => _name;
    public string Description => _description;
    public abstract InventoryItemType InventoryItemType { get; }
    public Sprite Icon => _icon;
}