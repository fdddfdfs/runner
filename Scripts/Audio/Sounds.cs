using System;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : ResourcesSingleton<Sounds, SoundsResourceName>
{
    [SerializeField] private List<AudioSource> _soundsSources;
    [SerializeField] private List<AudioClip> _soundsClips;

    private Dictionary<string, AudioClip> _clips;

    public void ChangeSoundsVolume(float newVolume)
    {
        foreach (AudioSource soundsSource in _soundsSources)
        {
            soundsSource.volume = newVolume;
        }
    }

    public void PlaySound(int layer, string clipName)
    {
        if (layer > _soundsSources.Count)
        {
            throw new IndexOutOfRangeException($"{layer} more then sounds sources count: {_soundsSources.Count}");
        }

        clipName = clipName.ToLower();

        if (!_clips.ContainsKey(clipName))
        {
            throw new Exception($"Clip with name: {clipName} doesnt exist in {nameof(Sounds)}");
        }
        
        _soundsSources[layer].clip = _clips[clipName];
        _soundsSources[layer].Play();
    }

    public void StopSound(int layer)
    {
        if (layer > _soundsSources.Count)
        {
            throw new IndexOutOfRangeException($"{layer} more then sounds sources count: {_soundsSources.Count}");
        }

        _soundsSources[layer].Stop();
    }

    private void Awake()
    {
        _clips = new Dictionary<string, AudioClip>();
        foreach (AudioClip soundsClip in _soundsClips)
        {
            _clips.Add(soundsClip.name.ToLower(), soundsClip);
        }
    }
}

public class SoundsResourceName : ResourceName
{
    public override string Name => "Audio/Sounds";
}