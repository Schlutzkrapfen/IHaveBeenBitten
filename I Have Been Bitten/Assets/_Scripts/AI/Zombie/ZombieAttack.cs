using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttack : BaseState
{
    Zombie zombie;
    public ZombieAttack(Character character, CharacterStateMachine controller, string animationName) : base(character, controller, animationName)
    {
        zombie = (Zombie)character;
    }
    public override void Enter()
    {
        base.Enter();
        character.GetAgent().isStopped = true;
        character.GetAudioSource().PlayOneShot(zombie.attackSound);
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

        if(character.GetTarget() == null || character.IsAnimationFinished()) 
        {
            controller.ChangeState(new ZombieIdle(character, controller, "Idle"));
            return;
        }

        Vector3 dir = character.GetTarget().transform.position - character.transform.position;
        dir.y = 0;

        character.transform.rotation = Quaternion.LookRotation(dir);
    }


}
