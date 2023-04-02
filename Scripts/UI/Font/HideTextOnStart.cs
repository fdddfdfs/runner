using UnityEngine;

public sealed class HideTextOnStart : ChangeTextOnStart
{
    public override void StartRun()
    {
        ChangeTextDilate(0, -1);
    }

    public override void EndRun()
    {
        Text.gameObject.SetActive(true);
        SetDilate(0);
    }
}