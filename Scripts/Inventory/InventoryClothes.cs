using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class InventoryClothes : InventoryGrid
{
    private readonly IClothesChanger _clothesChanger;

    public InventoryClothes(
        InventorySteamworks inventorySteamworks,
        List<GameObject> inventoryCells,
        TMP_Text nameText,
        TMP_Text descriptionText,
        Button button,
        Button nextPageButton,
        Button previousPageButton,
        IClothesChanger clothesChanger,
        Sprite invisibleSprite)
        : base(
            inventorySteamworks,
            inventoryCells,
            nameText,
            descriptionText,
            button,
            nextPageButton,
            previousPageButton,
            invisibleSprite)
    {
        _inventoryItemsType = InventoryItemType.Clothes;

        _clothesChanger = clothesChanger;
    }

    protected override void ShowItemInfo(int clotherNumber)
    {
        base.ShowItemInfo(clotherNumber);

        if (!_button.gameObject.activeSelf)
        {
            _button.gameObject.SetActive(true);
        }

        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(() =>
        {
            PutOnClother(clotherNumber);
        });

        _buttonText.text = Localization.Instance[AllTexts.PutClothes];
    }

    private void PutOnClother(int clotherNumber)
    {
        _clothesChanger.ChangeClothes(_items[clotherNumber].InventoryItemData.ID);
    }
}
