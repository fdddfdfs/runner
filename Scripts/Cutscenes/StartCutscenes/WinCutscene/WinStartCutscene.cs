using System;

public class WinStartCutscene : Cutscene
{
    private const float FadeMultiplier = 5f;
    
    private Action _endCutsceneCallback;

    protected override Action EndCutsceneCallback => _endCutsceneCallback;
    
    public void Init(Run run, Fade fade)
    {
        base.Init(fade);

        _endCutsceneCallback = () =>
        {
            fade.FadeIn(() =>
            {
                HideCutscene();
                run.StartRun();
                fade.FadeOut(null, FadeMultiplier);
            }, FadeMultiplier);
        };
    }
    
    public override void EndCutscene()
    {
        if (_isEnded) return;
        
        _endCutsceneCallback();
        TriggerEnd();
    }
}