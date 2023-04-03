using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class ActiveItem: MonoBehaviour
{
    [SerializeField] private Image _itemImage;
    [SerializeField] private List<Image> _progressImages;
    [SerializeField] private TMP_Text _text;

    public Image ItemImage => _itemImage;

    public List<Image> ProgressImages => _progressImages;

    public TMP_Text Text => _text;
}