using System;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cutscenes : MonoBehaviour
{
    private const string BaseStartCutsceneResourceName = "Cutscenes/BaseStartCutscene";

    private const string DeclineLoseCutsceneEnvironmentResourceName = "Cutscenes/DeclineLoseCutsceneEnvironment";
    private const string DeclineLoseStartCutsceneResourceName = "Cutscenes/DeclineLoseStartCutscene";
    private const string DeclineLoseEndCutsceneResourceName = "Cutscenes/DeclineLoseEndCutscene";

    [SerializeField] private Run _run;
    [SerializeField] private Fade _fade;
    [SerializeField] private MainMenu _mainMenu;
    [SerializeField] private ThirdPersonController _player;
    
    private Dictionary<Type, Cutscene> _cutscenes;
    private Cutscene _cutscene;
    private bool _cutsceneActive;

    public void ChangeCurrentCutscene(Type newCutscene)
    {
        _cutscene = _cutscenes[newCutscene];
        _cutscene.SetCutscene();
    }

    public void PlayCurrentCutscene()
    {
        _cutscene.PlayCutscene();
        _cutsceneActive = true;
    }
    
    public void ForceEndCutscene()
    {
        _cutscene.EndCutscene();
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
        declineLoseStartCutscene.Init(_run, _fade, declineLoseCutsceneEnvironment, _player);
        declineLoseEndCutscene.Init(_run, _fade, declineLoseCutsceneEnvironment, _mainMenu);
        
        declineLoseCutsceneEnvironment.ChangeEnvironmentActive(false);

        _cutscenes = new Dictionary<Type, Cutscene>();
        _cutscenes = new Dictionary<Type, Cutscene>
        {
            [typeof(BaseStartCutscene)] = baseStartCutscene,
            [typeof(DeclineLoseStartCutscene)] = declineLoseStartCutscene,
            [typeof(DeclineLoseEndCutscene)] = declineLoseEndCutscene,
        };
    }

    private void OnEnable()
    {
        foreach (Cutscene cutscene in _cutscenes.Values)
        {
            cutscene.OnCutsceneEnded += CutsceneEnded;
        }
    }

    private void OnDisable()
    {
        foreach (Cutscene cutscene in _cutscenes.Values)
        {
            cutscene.OnCutsceneEnded -= CutsceneEnded;
        }
    }

    private void Update()
    {
        if (!_cutsceneActive) return;
        
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            ForceEndCutscene();
        }
    }

    private void CutsceneEnded()
    {
        _cutsceneActive = false;
    }
}