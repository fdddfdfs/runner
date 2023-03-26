using System;

public sealed class BaseStartCutscene : Cutscene
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
            
            Achievements.Instance.GetAchievement("Start");
        };
    }

    public override void PlayCutscene()
    {
        base.PlayCutscene();
        
        Sounds.Instance.PlaySound(0, "BellKnock");
    }
}