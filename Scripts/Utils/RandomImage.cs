using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public sealed class RandomImage : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private List<Sprite> _sprites;

    private int _previous = -1;

    private void Awake()
    {
        _previous = Random.Range(0, _sprites.Count);
    }

    private void OnEnable()
    {
        _image.sprite = _sprites[_previous];
        _previous = (_previous + 1) % _sprites.Count;
    }
}