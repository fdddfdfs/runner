using System;
using UnityEngine;

public abstract class Cutscene : MonoBehaviour
{
    private const string StartCutsceneTrigger = "Start";
        
    [SerializeField] private Camera _cutsceneCamera;
    [SerializeField] private Animator _cutsceneAnimator;
    [SerializeField] private GameObject _cutsceneObject;

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
        _cutsceneObject.gameObject.SetActive(true);
    }

    public void HideCutscene()
    {
        _cutsceneCamera.gameObject.SetActive(false);
        _cutsceneObject.gameObject.SetActive(false);
    }

    public void PlayCutscene()
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