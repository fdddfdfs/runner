using System;
using UnityEngine;

public abstract class Cutscene : MonoBehaviour
{
    private const string StartCutsceneTrigger = "Start";
    private const string BackToStartTrigger = "Back";
        
    [SerializeField] protected Camera _cutsceneCamera;
    [SerializeField] private Animator _cutsceneAnimator;
    [SerializeField] private GameObject _cutsceneObject;
    [SerializeField] private float _endThreshold = 1f;

    private readonly int _startCutsceneTrigger = Animator.StringToHash(StartCutsceneTrigger);
    private readonly int _backToStartTrigger = Animator.StringToHash(BackToStartTrigger);

    private Fade _fade;

    protected bool _isEnded;

    public event Action OnCutsceneEnded;
    
    protected abstract Action EndCutsceneCallback { get; }

    protected void Init(Fade fade)
    {
        _fade = fade;
    }
    
    public virtual void SetCutscene()
    {
        _cutsceneCamera.gameObject.SetActive(true);
        _cutsceneObject.gameObject.SetActive(true);

        _isEnded = false;
    }

    public virtual void HideCutscene()
    {
        _cutsceneAnimator.SetTrigger(_backToStartTrigger);
        _cutsceneCamera.gameObject.SetActive(false);
        _cutsceneObject.gameObject.SetActive(false);
    }

    public virtual void PlayCutscene()
    {
        _cutsceneAnimator.SetTrigger(_startCutsceneTrigger); 
    }

    public virtual void EndCutscene()
    {
        if (_isEnded) return;
        
        _fade.FadeIn(EndCutsceneCallback);
        _isEnded = true;
        TriggerEnd();
    }

    protected void TriggerEnd()
    {
        OnCutsceneEnded?.Invoke();
    }

    private void Awake()
    {
        var endCutsceneBehaviour = _cutsceneAnimator.GetBehaviour<EndCutsceneBehaviour>();

        if (!endCutsceneBehaviour)
        {
            throw new NullReferenceException(
                $"{gameObject.name} animator must contain {nameof(EndCutsceneBehaviour)}");
        }
        
        endCutsceneBehaviour.Init(this, _endThreshold);
        _cutsceneAnimator.keepAnimatorControllerStateOnDisable = true;
        
        _cutsceneObject.SetActive(false);
    }
}