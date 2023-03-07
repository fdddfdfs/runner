using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class ActiveItem: MonoBehaviour
{
    [SerializeField] private Image _itemImage;
    [SerializeField] private List<Image> _progressImages;

    public Image ItemImage => _itemImage;

    public List<Image> ProgressImages => _progressImages;
}