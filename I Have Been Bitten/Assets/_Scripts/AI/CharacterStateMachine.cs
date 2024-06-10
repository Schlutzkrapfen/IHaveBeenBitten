using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateMachine
{
    BaseState currentState;

    public void InitController(BaseState startState)
    {
        currentState = startState;
        currentState.Enter();
    }
    public void ChangeState(BaseState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void UpdateState()
    {
        currentState.StateUpdate();
    }

    public BaseState GetState()
    {
        return currentState;
    }
}
