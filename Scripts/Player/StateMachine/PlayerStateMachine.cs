using System;
using System.Collections.Generic;
using StarterAssets;

public sealed class PlayerStateMachine : IRunnable
{
    private IState _currentState;

    private readonly Dictionary<Type, IState> _states;

    public PlayerStateMachine(
        ThirdPersonController player,
        ActiveItemsUI activeItemsUI,
        Follower follower,
        Effects effects)
    {
        _states = new Dictionary<Type, IState>
        {
            [typeof(RunState)] = new RunState(player),
            [typeof(FlyState)] = new FlyState(player, activeItemsUI, follower, effects),
            [typeof(ImmuneState)] = new ImmuneState(player, activeItemsUI, effects),
            [typeof(BoardState)] = new BoardState(player, activeItemsUI),
            [typeof(IdleState)] = new IdleState(player.PlayerAnimator),
        };

        _currentState = _states[typeof(IdleState)];
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

    public void StartRun()
    {
        ChangeState(typeof(RunState));
    }

    public void EndRun()
    {
        ChangeState(typeof(IdleState));
    }
}