using System;
using UnityEngine;

public class Fade : MonoBehaviour
{
    private const string FadeInTrigger = "FadeIn";
    private const string FadeOutTrigger = "FadeOut";
    
    [SerializeField] private GameObject _fadeObject;
    [SerializeField] private Animator _fadeAnimator;

    private readonly int _fadeInTrigger = Animator.StringToHash(FadeInTrigger);
    private readonly int _fadeOutTrigger = Animator.StringToHash(FadeOutTrigger);

    private Action _fadeInCallback;
    private Action _fadeOutCallback;
    
    
    public void FadeIn(Action callback)
    {
        _fadeObject.SetActive(true);
        _fadeInCallback = callback;
        _fadeAnimator.SetTrigger(_fadeInTrigger);
    }

    public void FadeOut(Action callback)
    {
        _fadeOutCallback = callback;
        _fadeAnimator.SetTrigger(_fadeOutTrigger);
    }

    public void FinishFadeIn()
    {
        _fadeInCallback?.Invoke();
    }

    public void FinishFadeOut()
    {
        _fadeOutCallback?.Invoke();
        _fadeObject.SetActive(false);
    }

    private void Awake()
    {
        _fadeAnimator.GetBehaviour<FadeOutBehaviour>().Fade = this;
        _fadeAnimator.GetBehaviour<FadeInBehaviour>().Fade = this;
        _fadeObject.SetActive(false);
    }
}