using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public sealed class RandomImage : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private List<Sprite> _sprites;

    private int _previous = -1;
    
    private void OnEnable()
    {
        int r;
        do
        {
            r = Random.Range(0, _sprites.Count);
        } while (_sprites.Count > 1 && r == _previous);
        
        _image.sprite = _sprites[r];
        _previous = r;
    }
}