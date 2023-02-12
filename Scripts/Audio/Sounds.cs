using System;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : ResourcesSingleton<Sounds, SoundsResourceName>
{
    [SerializeField] private List<AudioSource> _soundsSources;
    [SerializeField] private List<AudioClip> _soundsClips;

    public void ChangeSoundsVolume(float newVolume)
    {
        foreach (AudioSource soundsSource in _soundsSources)
        {
            soundsSource.volume = newVolume;
        }
    }

    public void PlaySound(int layer, int clip)
    {
        if (layer > _soundsSources.Count)
        {
            throw new IndexOutOfRangeException($"{layer} more then sounds sources count: {_soundsSources.Count}");
        }

        if (clip > _soundsClips.Count)
        {
            throw new IndexOutOfRangeException($"{clip} more then sounds clip count: {_soundsClips.Count}");
        }

        _soundsSources[layer].clip = _soundsClips[clip];
        _soundsSources[layer].Play();
    }
}

public class SoundsResourceName : ResourceName
{
    public override string Name => "Audio/Sounds";
}