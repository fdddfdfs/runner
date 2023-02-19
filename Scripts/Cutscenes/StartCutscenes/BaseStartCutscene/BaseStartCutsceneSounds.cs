using UnityEngine;
public class BaseStartCutsceneSounds : MonoBehaviour
{
    private void PlayWrongDoorSounds()
    {
        Sounds.Instance.PlaySound(1,"WrongDoor");
    }

    private void PlayWalkSound(int layer)
    {
        Sounds.Instance.PlaySound(layer,"WoodWalk");
    }

    private void PlaySubpoenaSound()
    {
        Sounds.Instance.PlaySound(0,"Subpoena");
    }

    private void PlayHardKickSound()
    {
        Sounds.Instance.PlaySound(1,"HardKnock");
    }

    private void PlayJumpSound()
    {
        Sounds.Instance.PlaySound(2, "Jump");
    }

    private void StopWalkSound(int layer)
    {
        Sounds.Instance.StopSound(layer);
    }
}