using System;

public sealed class AcceptLoseEndCutscene : CutsceneWithEnvironment
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
            mainMenu.SetCutsceneType(typeof(AcceptLoseStartCutscene));
            mainMenu.ShowMainMenu();
        };
    }
}