using System;

public class PrisonEndGameCutscene : Cutscene
{
    private Action _endCutsceneCallback;

    protected override Action EndCutsceneCallback => _endCutsceneCallback;
    
    public void Init(Run run ,Fade fade)
    {
        base.Init(fade);

        _endCutsceneCallback = () =>
        {
            HideCutscene();
            fade.FadeOut(null);
            run.StartRun();
        };
    }
}