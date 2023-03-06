using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public sealed class ActiveItem: MonoBehaviour
{
    [SerializeField] private Image _itemImage;
    [FormerlySerializedAs("_progressImage")] [SerializeField] private List<Image> _progressImages;

    public Image ItemImage => _itemImage;

    public List<Image> ProgressImages => _progressImages;
}