using System;

public class DeclineLoseStartCutscene : Cutscene
{
    private Action _endCutsceneCallback;
    private CutsceneEnvironment _cutsceneEnvironment;

    protected override Action EndCutsceneCallback => _endCutsceneCallback;
    
    public void Init(Run run ,Fade fade, CutsceneEnvironment cutsceneEnvironment)
    {
        base.Init(fade);
        _cutsceneEnvironment = cutsceneEnvironment;

        _endCutsceneCallback = () =>
        {
            HideCutscene();
            fade.FadeOut(null);
            run.StartRun();
            _cutsceneEnvironment.ChangeEnvironmentActive(false);
        };
    }

    public override void SetCutscene()
    {
        base.SetCutscene();
        
        _cutsceneEnvironment.ChangeEnvironmentActive(true);
    }
}