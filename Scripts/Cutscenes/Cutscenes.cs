using System;
using System.Collections.Generic;
using UnityEngine;

public class Cutscenes : MonoBehaviour
{
    private const string BaseStartCutsceneResourceName = "Cutscenes/BaseStartCutscene";

    [SerializeField] private Run _run;
    [SerializeField] private Fade _fade;
    
    private Dictionary<Type, Cutscene> _cutscenes;
    private Cutscene _cutscene;

    public void ChangeCurrentCutscene(Type newCutscene)
    {
        _cutscene = _cutscenes[newCutscene];
        _cutscene.SetCutscene();
    }

    public void PlayCurrentCutscene()
    {
        _cutscene.PlayCutscene();
    }
    
    private void Awake()
    {
        var baseStartCutscene = ResourcesLoader.InstantiateLoadComponent<BaseStartCutscene>(
            BaseStartCutsceneResourceName);

        baseStartCutscene.Init(_run, _fade);

        _cutscenes = new Dictionary<Type, Cutscene>
        {
            [typeof(BaseStartCutscene)] = baseStartCutscene,
        };
    }
}