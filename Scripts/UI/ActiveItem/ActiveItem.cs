using UnityEngine;
using UnityEngine.UI;

public class ActiveItem: MonoBehaviour
{
    [SerializeField] private Image _itemImage;
    [SerializeField] private Image _progressImage;

    public Image ItemImage => _itemImage;

    public Image ProgressImage => _progressImage;
}