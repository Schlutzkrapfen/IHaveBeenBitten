using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieIdle : BaseState
{
    Zombie zombie;
    
    public ZombieIdle(Character character, CharacterStateMachine controller, string animationName) : base(character,controller, animationName)
    {
        zombie = (Zombie)character;
    }

    public override void Enter()
    {
        base.Enter();
        character.GetAgent().isStopped = true;
        character.FindTarget();
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

        if (character.FindTarget() != null)
        {
            if(character.GetDistanceFromTarget() <= zombie.attackDistance)
            {
                controller.ChangeState(new ZombieAttack(character, controller, "Attack"));
            }
            else
            {
                controller.ChangeState(new ZombieChase(character, controller, "Walk"));
            }
        }
    }
}
