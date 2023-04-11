using UnityEngine;

public class EnvironmentSound : MonoBehaviour, ITriggerable
{
    private const int EnvironmentSoundLayer = 3;

    [SerializeField] private string _soundName;
    [SerializeField] private bool _isRandomSound;

    public void Trigger()
    {
        if (_isRandomSound)
        {
            Sounds.Instance.PlayRandomSounds(EnvironmentSoundLayer, _soundName);
        }
        else
        {
            Sounds.Instance.PlaySound(EnvironmentSoundLayer, _soundName);
        }
    }
}