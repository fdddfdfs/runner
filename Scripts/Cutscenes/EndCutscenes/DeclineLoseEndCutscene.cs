﻿using System;

public class DeclineLoseEndCutscene : Cutscene
{
    private Action _endCutsceneCallback;
    private CutsceneEnvironment _cutsceneEnvironment;

    protected override Action EndCutsceneCallback => _endCutsceneCallback;
    
    public void Init(Run run ,Fade fade, CutsceneEnvironment cutsceneEnvironment, MainMenu mainMenu)
    {
        base.Init(fade);
        _cutsceneEnvironment = cutsceneEnvironment;

        _endCutsceneCallback = () =>
        {
            HideCutscene();
            fade.FadeOut(null);
            _cutsceneEnvironment.ChangeEnvironmentActive(false);
            mainMenu.SetCutsceneType(typeof(DeclineLoseStartCutscene));
            mainMenu.ShowMainMenu();
        };
    }

    public override void SetCutscene()
    {
        base.SetCutscene();
        
        _cutsceneEnvironment.ChangeEnvironmentActive(true);
    }
}