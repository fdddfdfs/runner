using System;
using TMPro;
using Unity.VisualScripting;
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

    public event Action<int> OnSelect;

    private string _itemName;
    private string _itemDescription;

    public void Init(
        Sprite itemSprite,
        string itemName,
        string itemDescription,
        Func<int> getLevel,
        Action increaseLevel,
        int index)
    {
        _itemName = itemName;
        _itemDescription = itemDescription;
        _image.sprite = itemSprite;
        LocalizeText();
        Localization.Instance.OnLanguageUpdated += LocalizeText;
        
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
            if (price > Stats.Money.Value)
            {
                Sounds.Instance.PlayRandomSounds(2, "NotEnoughMoney");
                return;
            }

            Stats.Money.Value -= price;
            increaseLevel.Invoke();

            Instantiate(_firstLevel, _firstLevel.transform.parent);
            _price.text = CalculateUpgradePrice(level + 1).ToString();
            
            if (level + 1 == Stats.MaxLevel)
            {
                _buy.gameObject.SetActive(false);
            }
            
            Sounds.Instance.PlayRandomSounds(2, "Upgrade");
        });

        _buy.AddComponent<OnSelectButton>().Init(() => OnSelect?.Invoke(index));
    }
    
    public void SelectButton()
    {
        _buy.Select();
    }

    //TODO: make function to calculate upgrade price
    private static int CalculateUpgradePrice(int level)
    {
        return level * 100;
    }

    private void LocalizeText()
    {
        _name.text = Localization.Instance[_itemName];
        _description.text = Localization.Instance[_itemDescription];
    }
}