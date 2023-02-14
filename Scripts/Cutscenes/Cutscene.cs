using System;
using UnityEngine;

public abstract class Cutscene : MonoBehaviour
{
    private const string StartCutsceneTrigger = "Start";
        
    [SerializeField] private Camera _cutsceneCamera;
    [SerializeField] private Animator _cutsceneAnimator;

    private readonly int _startCutsceneTrigger = Animator.StringToHash(StartCutsceneTrigger);
    
    private Fade _fade;

    protected abstract Action EndCutsceneCallback { get; }

    protected void Init(Fade fade)
    {
        _fade = fade;
    }
    
    public void SetCutscene()
    {
        _cutsceneCamera.gameObject.SetActive(true);
    }

    public async void PlayCutscene()
    {
        _cutsceneAnimator.SetTrigger(_startCutsceneTrigger);
    }

    public void EndCutscene()
    {
        _fade.FadeIn(EndCutsceneCallback);
    }

    private void Awake()
    {
        _cutsceneAnimator.GetBehaviour<EndCutsceneBehaviour>().Cutscene = this;
    }
}