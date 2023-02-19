using System;
using System.Collections.Generic;
using UnityEngine;

public class Cutscenes : MonoBehaviour
{
    private const string BaseStartCutsceneResourceName = "Cutscenes/BaseStartCutscene";

    private const string DeclineLoseCutsceneEnvironmentResourceName = "Cutscenes/DeclineLoseCutsceneEnvironment";
    private const string DeclineLoseStartCutsceneResourceName = "Cutscenes/DeclineLoseStartCutscene";
    private const string DeclineLoseEndCutsceneResourceName = "Cutscenes/DeclineLoseEndCutscene";

    [SerializeField] private Run _run;
    [SerializeField] private Fade _fade;
    [SerializeField] private MainMenu _mainMenu;
    
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
        var baseStartCutscene = 
            ResourcesLoader.InstantiateLoadComponent<BaseStartCutscene>(BaseStartCutsceneResourceName);
        
        var declineLoseCutsceneEnvironment =
            ResourcesLoader.InstantiateLoadComponent<CutsceneEnvironment>(DeclineLoseCutsceneEnvironmentResourceName);
        var declineLoseStartCutscene =
            ResourcesLoader.InstantiateLoadComponent<DeclineLoseStartCutscene>(DeclineLoseStartCutsceneResourceName);
        var declineLoseEndCutscene =
            ResourcesLoader.InstantiateLoadComponent<DeclineLoseEndCutscene>(DeclineLoseEndCutsceneResourceName);

        baseStartCutscene.Init(_run, _fade);
        declineLoseStartCutscene.Init(_run, _fade, declineLoseCutsceneEnvironment);
        declineLoseEndCutscene.Init(_run, _fade, declineLoseCutsceneEnvironment, _mainMenu);

        _cutscenes = new Dictionary<Type, Cutscene>();
        _cutscenes = new Dictionary<Type, Cutscene>
        {
            [typeof(BaseStartCutscene)] = baseStartCutscene,
            [typeof(DeclineLoseStartCutscene)] = declineLoseStartCutscene,
            [typeof(DeclineLoseEndCutscene)] = declineLoseEndCutscene,
        };
    }
}