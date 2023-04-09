using System;
using StarterAssets;
using UnityEngine;

public class DeclineLoseStartCutscene : CutsceneWithEnvironment
{
    private const float FadeMultiplier = 5f;
    
    [SerializeField] private Transform _player;
    
    private Action _endCutsceneCallback;

    protected override Action EndCutsceneCallback => _endCutsceneCallback;
    
    public void Init(Run run, Fade fade, CutsceneEnvironment cutsceneEnvironment, ThirdPersonController player)
    {
        base.Init(fade, cutsceneEnvironment);

        _endCutsceneCallback = () =>
        {
            fade.FadeIn(() =>
            {
                HideCutscene();
                Transform cameraTransform = _cutsceneCamera.transform;
                player.SetStartRunPosition(
                    _player.transform.position,
                    cameraTransform.position,
                    cameraTransform.rotation);
                
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