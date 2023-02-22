using UnityEngine;

public class CutsceneSounds : MonoBehaviour
{
    private void PlayLayerOneSound(string soundName)
    {
        Sounds.Instance.PlaySound(1, soundName);
    }

    private void PlayLayerTwoSound(string soundName)
    {
        Sounds.Instance.PlaySound(2, soundName);
    }

    private void PlayLayerThreeSound(string soundName)
    {
        Sounds.Instance.PlaySound(3, soundName);
    }

    private void StopSound(int layer)
    {
        Sounds.Instance.StopSound(layer);
    }
}