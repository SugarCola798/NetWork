using System;
using System.Collections.Generic;

public sealed class StateMachine<TStateId>
{
    private readonly Dictionary<TStateId, IState> states = new Dictionary<TStateId, IState>();
    private readonly Dictionary<TStateId, List<Transition>> transitions = new Dictionary<TStateId, List<Transition>>();
    private readonly List<Transition> anyTransitions = new List<Transition>();

    private IState currentState;
    private TStateId currentStateId;
    private bool isInitialized;

    public TStateId CurrentStateId => currentStateId;

    public void AddState(TStateId stateId, IState state)
    {
        states[stateId] = state;
    }

    public void AddTransition(TStateId from, TStateId to, Func<bool> condition)
    {
        if (!transitions.TryGetValue(from, out var transitionList))
        {
            transitionList = new List<Transition>();
            transitions[from] = transitionList;
        }

        transitionList.Add(new Transition(to, condition));
    }

    public void AddAnyTransition(TStateId to, Func<bool> condition)
    {
        anyTransitions.Add(new Transition(to, condition));
    }

    public void SetInitialState(TStateId initialStateId)
    {
        if (!states.ContainsKey(initialStateId))
        {
            throw new ArgumentException($"State not found: {initialStateId}");
        }

        if (isInitialized)
        {
            ChangeState(initialStateId);
            return;
        }

        currentStateId = initialStateId;
        currentState = states[initialStateId];
        currentState.Enter();
        isInitialized = true;
    }

    public void Tick()
    {
        if (!isInitialized)
        {
            return;
        }

        if (TryGetNextTransition(out var nextStateId))
        {
            ChangeState(nextStateId);
        }

        currentState?.Tick();
    }

    private bool TryGetNextTransition(out TStateId nextStateId)
    {
        foreach (var transition in anyTransitions)
        {
            if (transition.Condition())
            {
                nextStateId = transition.To;
                return true;
            }
        }

        if (transitions.TryGetValue(currentStateId, out var stateTransitions))
        {
            foreach (var transition in stateTransitions)
            {
                if (transition.Condition())
                {
                    nextStateId = transition.To;
                    return true;
                }
            }
        }

        nextStateId = default;
        return false;
    }

    private void ChangeState(TStateId nextStateId)
    {
        if (EqualityComparer<TStateId>.Default.Equals(currentStateId, nextStateId))
        {
            return;
        }

        if (!states.TryGetValue(nextStateId, out var nextState))
        {
            throw new ArgumentException($"State not found: {nextStateId}");
        }

        currentState?.Exit();
        currentStateId = nextStateId;
        currentState = nextState;
        currentState.Enter();
    }

    private sealed class Transition
    {
        public Transition(TStateId to, Func<bool> condition)
        {
            To = to;
            Condition = condition;
        }

        public TStateId To { get; }
        public Func<bool> Condition { get; }
    }
}
