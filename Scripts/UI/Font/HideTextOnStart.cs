public sealed class HideTextOnStart : ChangeTextOnStart
{
    public override void StartRun()
    {
        ChangeTextDilate(0, -1);
    }

    public override void EndRun()
    {
        ChangeTextDilate(-1, 1);
    }
}