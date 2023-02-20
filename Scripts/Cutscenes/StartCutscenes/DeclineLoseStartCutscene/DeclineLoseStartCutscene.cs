using System;
using StarterAssets;
using UnityEngine;

public class DeclineLoseStartCutscene : Cutscene
{
    [SerializeField] private Transform _player;
    
    private Action _endCutsceneCallback;
    private CutsceneEnvironment _cutsceneEnvironment;

    protected override Action EndCutsceneCallback => _endCutsceneCallback;
    
    public void Init(Run run ,Fade fade, CutsceneEnvironment cutsceneEnvironment, ThirdPersonController player)
    {
        base.Init(fade);
        _cutsceneEnvironment = cutsceneEnvironment;

        _endCutsceneCallback = () =>
        {
            HideCutscene();
            Transform cameraTransform = _cutsceneCamera.transform;
            player.SetStartRunPosition(
                _player.transform.position,
                cameraTransform.position,
                cameraTransform.rotation);
            run.StartRun();
        };
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

    public override void EndCutscene()
    {
        if (_isEnded) return;
        
        _endCutsceneCallback();
        TriggerEnd();
    }
}