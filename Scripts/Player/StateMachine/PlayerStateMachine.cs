﻿using System;
using System.Collections.Generic;
using StarterAssets;

public class PlayerStateMachine
{
    private IState _currentState;

    private readonly Dictionary<Type, IState> _states;

    public PlayerStateMachine(ThirdPersonController player, ActiveItemsUI activeItemsUI)
    {
        _states = new Dictionary<Type, IState>
        {
            [typeof(RunState)] = new RunState(player),
            [typeof(FlyState)] = new FlyState(player, activeItemsUI),
            [typeof(ImmuneState)] = new ImmuneState(player, activeItemsUI),
            [typeof(BoardState)] = new BoardState(player, activeItemsUI),
        };

        _currentState = _states[typeof(RunState)];
        _currentState.EnterState();
    }

    public void ChangeState(Type newState)
    {
        _currentState.ExitState();
        _currentState = _states[newState];
        _currentState.EnterState();
    }

    public void ChangeStateSafely(Type from, Type to)
    {
        if (_currentState.GetType() != from)
        {
            return;
        }
        
        ChangeState(to);
    }
}