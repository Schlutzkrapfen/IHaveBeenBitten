using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieChase : BaseState
{
    Zombie zombie;
    public ZombieChase(Character character, CharacterStateMachine controller, string animationName) : base(character, controller, animationName)
    {
        zombie = (Zombie)character;
    }
    public override void Enter()
    {
        base.Enter();
        character.GetAgent().isStopped = false;
        character.GetAudioSource().clip = zombie.chaseSound;
        character.GetAudioSource().loop = true;
        character.GetAudioSource().Play();
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
        if(character.GetTarget() != null) 
        {
            //set destination
            if (character.GetAgent().SetDestination(character.GetTarget().position))
            {
                if (character.GetAgent().hasPath)
                {
                    //rotate towards the first corner of the path, move with root motion
                    Vector3 dir = character.GetAgent().path.corners[1] - character.transform.position;
                    dir.y = 0;
                    Quaternion targetRotation = Quaternion.LookRotation(dir);
                    // Smoothly rotate towards the target rotation
                    character.transform.rotation = Quaternion.Slerp(character.transform.rotation, targetRotation, character.GetAgent().angularSpeed * Time.deltaTime);

                }
            }
            else
            {
                Debug.Log("failed to set destination");
            }
            if (character.GetDistanceFromTarget() <= zombie.attackDistance) 
            {
                controller.ChangeState(new ZombieAttack(character, controller, "Attack"));
            }

        }
        else
        {
            if(character.DebugCharacter) Debug.Log("Target null");
            controller.ChangeState(new ZombieIdle(character, controller,"Idle"));
        }
    }
}
