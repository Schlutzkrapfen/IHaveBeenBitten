using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralHumanIdle : BaseState
{
    NeutralHuman neutralHuman;
    public NeutralHumanIdle(Character character, CharacterStateMachine controller, string animationName) : base(character, controller, animationName)
    {
        neutralHuman = (NeutralHuman)character;
    }
    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit() 
    { 
        base.Exit();
    }
    public override void CheckTransition()
    {
        base.CheckTransition();
    }
    public override void StateUpdate()
    {
        base.StateUpdate();

        if(neutralHuman.isBitten)
        {
            controller.ChangeState(new NeutralHumanTransform(neutralHuman, controller, "Transform"));
        }

        int chance = Random.Range(0, 100);
        if(chance > neutralHuman.fearPercentage && neutralHuman.ShouldRun)
        {
            //wander state
            controller.ChangeState(new NeutralHumanWander(neutralHuman, controller, "Wander"));
        }
        else
        {
            //fear state
            controller.ChangeState(new NeutralHumanFear(neutralHuman, controller, "Terrified"));
        }

    }
}
