using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Sounds : ResourcesSingleton<Sounds, SoundsResourceName>
{
    [SerializeField] private List<AudioSource> _soundsSources;
    [SerializeField] private List<AudioClip> _soundsClips;

    private Dictionary<string, AudioClip> _clips;
    private Dictionary<string, int> _randomClipsCount;

    public void ChangeSoundsVolume(float newVolume)
    {
        foreach (AudioSource soundsSource in _soundsSources)
        {
            soundsSource.volume = newVolume;
        }
    }

    public void PlaySound(int layer, string clipName)
    {
        CheckLayer(layer);

        clipName = clipName.ToLower();

        if (!_clips.ContainsKey(clipName))
        {
            throw new Exception($"Clip with name: {clipName} doesnt exist in {nameof(Sounds)}");
        }
        
        _soundsSources[layer].clip = _clips[clipName];
        _soundsSources[layer].Play();
    }

    public void PlayRandomSounds(int layer, string clipBaseName)
    {
        CheckLayer(layer);
        
        clipBaseName = clipBaseName.ToLower();
        
        if (!_randomClipsCount.ContainsKey(clipBaseName))
        {
            throw new Exception($"{clipBaseName} doesnt exist int {nameof(Sounds)}");
        }

        int r = Random.Range(0, _randomClipsCount[clipBaseName]);

        string clipName = clipBaseName + r;

        if (!_clips.ContainsKey(clipName))
        {
            throw new Exception(
                $"Clip with name: {clipName} doesnt exist in {nameof(Sounds)}," +
                $" clips with {clipBaseName} probably have missing numbers");
        }
        
        _soundsSources[layer].clip = _clips[clipName];
        _soundsSources[layer].Play();
    }

    public void StopSound(int layer)
    {
        CheckLayer(layer);

        _soundsSources[layer].Stop();
    }

    public void StopAllSounds()
    {
        foreach (AudioSource soundsSource in _soundsSources)
        {
            soundsSource.Stop();
        }
    }

    private void Awake()
    {
        _randomClipsCount = new Dictionary<string, int>();
        _clips = new Dictionary<string, AudioClip>();
        foreach (AudioClip soundsClip in _soundsClips)
        {
            string clipName = soundsClip.name.ToLower();
            
            if (char.IsDigit(clipName[^1]))
            {
                int index = clipName.Length - 1;
                while (char.IsDigit(clipName[index - 1]))
                {
                    index--;
                }

                string baseName = clipName[index..];

                if (_randomClipsCount.ContainsKey(baseName))
                {
                    _randomClipsCount[baseName] += 1;
                }
                else
                {
                    _randomClipsCount[baseName] = 1;
                }
            }
            
            _clips.Add(clipName, soundsClip);
        }
    }

    private void CheckLayer(int layer)
    {
        if (layer > _soundsSources.Count)
        {
            throw new IndexOutOfRangeException($"{layer} more then sounds sources count: {_soundsSources.Count}");
        }
    }
}

public class SoundsResourceName : ResourceName
{
    public override string Name => "Audio/Sounds";
}