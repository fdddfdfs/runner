public sealed class ShowTextOnStart : ChangeTextOnStart
{
    public override void StartRun()
    {
        ChangeTextDilate(-1, 1);
    }

    public override void EndRun()
    {
        ChangeTextDilate(0, -1);
    }
}