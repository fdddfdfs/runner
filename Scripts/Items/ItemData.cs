using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Items/Item")]
public sealed class ItemData : ScriptableObject
{
    [SerializeField] private ItemType _type;
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private Sprite _icon;

    public ItemType Type => _type;
    public string Name => _name;
    public string Description => _description;
    public Sprite Icon => _icon;
}