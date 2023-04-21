using System;
using StarterAssets;
using UnityEngine;

public class WinCutscene : Cutscene
{
    private Action _endCutsceneCallback;

    protected override Action EndCutsceneCallback => _endCutsceneCallback;
    
    public void Init(ThirdPersonController player, Run run, Fade fade)
    {
        base.Init(fade);

        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;
        
        _endCutsceneCallback = () =>
        {
            HideCutscene();
            fade.FadeOut(null);
            run.SetMainMenuCutscene(typeof(WinStartCutscene));
            run.BackToMenu(false);
        };
    }
}