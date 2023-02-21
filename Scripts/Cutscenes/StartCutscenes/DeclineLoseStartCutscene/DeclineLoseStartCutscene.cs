using System;
using StarterAssets;
using UnityEngine;

public class DeclineLoseStartCutscene : CutsceneWithEnvironment
{
    [SerializeField] private Transform _player;
    
    private Action _endCutsceneCallback;

    protected override Action EndCutsceneCallback => _endCutsceneCallback;
    
    public void Init(Run run ,Fade fade, CutsceneEnvironment cutsceneEnvironment, ThirdPersonController player)
    {
        base.Init(fade, cutsceneEnvironment);

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
    
    public override void EndCutscene()
    {
        if (_isEnded) return;
        
        _endCutsceneCallback();
        TriggerEnd();
    }
}