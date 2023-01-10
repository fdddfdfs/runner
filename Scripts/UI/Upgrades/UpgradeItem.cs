using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class UpgradeItem : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private GameObject _firstLevel;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private TMP_Text _price;
    [SerializeField] private Button _buy;

    public void Init(Sprite itemSprite, string itemName, string itemDescription, Func<int> getLevel, Action increaseLevel)
    {
        _image.sprite = itemSprite;
        _name.text = itemName;
        _description.text = itemDescription;
        int currentLevel = getLevel.Invoke();

        for (int i = 0; i < currentLevel - 1; i++)
        {
            Instantiate(_firstLevel, _firstLevel.transform.parent);
        }
        
        if (currentLevel == Stats.MaxLevel)
        {
            _buy.gameObject.SetActive(false);
            return;
        }
        
        _price.text = CalculateUpgradePrice(currentLevel).ToString();
        _buy.onClick.AddListener(() =>
        {
            int level = getLevel.Invoke();
            int price = CalculateUpgradePrice(level);
            if (price > Stats.Instance.Money) return;

            Stats.Instance.Money -= price;
            increaseLevel.Invoke();
            
            Instantiate(_firstLevel, _firstLevel.transform.parent);
            _price.text = CalculateUpgradePrice(currentLevel).ToString();
        });
    }

    //TODO: make function to calculate upgrade price
    private int CalculateUpgradePrice(int level)
    {
        return level * 1000;
    }
}