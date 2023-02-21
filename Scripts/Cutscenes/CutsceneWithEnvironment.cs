using UnityEngine;

public abstract class CutsceneWithEnvironment : Cutscene
{
    private CutsceneEnvironment _cutsceneEnvironment;
    
    protected void Init(Fade fade, CutsceneEnvironment cutsceneEnvironment)
    {
        base.Init(fade);

        _cutsceneEnvironment = cutsceneEnvironment;
    }
    
    public override void SetCutscene()
    {
        base.SetCutscene();
        
        _cutsceneEnvironment.ChangeEnvironmentActive(true);
    }

    public override void HideCutscene()
    {
        base.HideCutscene();
        
        _cutsceneEnvironment.ChangeEnvironmentActive(false);
    }
}