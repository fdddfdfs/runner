using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewItem : MonoBehaviour
{
    private const string AppearTrigger = "Appear";
    private const string HideTrigger = "Hide";
    
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _newItemText;
    [SerializeField] private TMP_Text _itemName;
    [SerializeField] private TMP_Text _itemDescription;
    [SerializeField] private Animator _animator;

    private static readonly int AppearTriggerID = Animator.StringToHash(AppearTrigger);
    private static readonly int HideTriggerID = Animator.StringToHash(HideTrigger);

    private string _startNewItemText;

    public async void Appear(float stayTime, InventoryItemData itemData)
    {
        _icon.sprite = itemData.Icon;
        _itemName.text = Localization.Instance[itemData.Name];
        _itemDescription.text = Localization.Instance[itemData.Description];
        _newItemText.text = Localization.Instance[_startNewItemText];

        _animator.SetTrigger(AppearTriggerID);

        CancellationToken token = AsyncUtils.Instance.GetCancellationToken();
        
        await AsyncUtils.Wait(stayTime, token, true);

        if (token.IsCancellationRequested) return;
        
        _animator.SetTrigger(HideTriggerID);
    }

    private void Awake()
    {
        _startNewItemText = _newItemText.text;
        
        _animator.GetBehaviour<EndStateBehaviour>().Init(() =>
        {
            gameObject.SetActive(false);
        });

        _animator.keepAnimatorControllerStateOnDisable = true;
    }
}