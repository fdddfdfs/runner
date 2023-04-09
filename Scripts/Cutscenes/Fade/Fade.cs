using System;
using UnityEngine;

public sealed class Fade : MonoBehaviour
{
    private const string FadeInTrigger = "FadeIn";
    private const string FadeOutTrigger = "FadeOut";
    private const string FadeMultiplier = "FadeMultiplier";
    
    [SerializeField] private GameObject _fadeObject;
    [SerializeField] private Animator _fadeAnimator;

    private readonly int _fadeInTrigger = Animator.StringToHash(FadeInTrigger);
    private readonly int _fadeOutTrigger = Animator.StringToHash(FadeOutTrigger);
    private readonly int _fadeMultiplier = Animator.StringToHash(FadeMultiplier);

    private Action _fadeInCallback;
    private Action _fadeOutCallback;
    
    
    public void FadeIn(Action callback, float fadeMultiplier = 1f)
    {
        _fadeObject.SetActive(true);
        _fadeInCallback = callback;
        _fadeAnimator.SetFloat(_fadeMultiplier, fadeMultiplier);
        _fadeAnimator.SetTrigger(_fadeInTrigger);
    }

    public void FadeOut(Action callback, float fadeMultiplier = 1f)
    {
        _fadeOutCallback = callback;
        _fadeAnimator.SetFloat(_fadeMultiplier, fadeMultiplier);
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