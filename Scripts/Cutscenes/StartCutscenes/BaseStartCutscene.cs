using System;

public class BaseStartCutscene : Cutscene
{
    private Action _endCutsceneCallback;

    protected override Action EndCutsceneCallback => _endCutsceneCallback;

    public void Init(Run run ,Fade fade)
    {
        base.Init(fade);

        _endCutsceneCallback = () =>
        {
            run.StartRun();
            fade.FadeOut(null);
        };
    }
}