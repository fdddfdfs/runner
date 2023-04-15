using UnityEngine;

[CreateAssetMenu(fileName = "SoundData", menuName = "Sounds/SoundData")]
public sealed class SoundData : ScriptableObject
{
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private float _soundVolume = 1f;

    public AudioClip AudioClip => _audioClip;
    public float SoundVolume => _soundVolume;
}