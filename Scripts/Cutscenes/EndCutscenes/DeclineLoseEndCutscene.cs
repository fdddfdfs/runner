using System;

public sealed class DeclineLoseEndCutscene : CutsceneWithEnvironment
{
    private Action _endCutsceneCallback;

    protected override Action EndCutsceneCallback => _endCutsceneCallback;
    
    public void Init(Fade fade, CutsceneEnvironment cutsceneEnvironment, MainMenu mainMenu)
    {
        base.Init(fade, cutsceneEnvironment);

        _endCutsceneCallback = () =>
        {
            HideCutscene();
            fade.FadeOut(null);
            mainMenu.SetCutsceneType(typeof(DeclineLoseStartCutscene));
            mainMenu.ShowMainMenu();
            
            Achievements.Instance.GetAchievement("Decline");
        };
    }
}