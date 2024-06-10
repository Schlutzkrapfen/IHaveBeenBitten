using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class NeutralHumanWander : BaseState
{
    NeutralHuman neutralHuman;
    Transform player;
    public NeutralHumanWander(Character neutralHuman, CharacterStateMachine controller,string animationName) : base(neutralHuman, controller,animationName)
    {
        this.neutralHuman = (NeutralHuman) neutralHuman;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override void Enter()
    {
        base.Enter();
        character.GetAnimator().SetTrigger(animationName);
        neutralHuman.GetAgent().isStopped = false;

    }
    public override void Exit()
    {
        base.Exit();   
    }
    public override void CheckTransition()
    {
        base.CheckTransition();
        if(neutralHuman.isBitten)
        {
            controller.ChangeState(new NeutralHumanTransform(neutralHuman, controller, "Transform"));
        }
        if(Vector3.Distance(neutralHuman.transform.position, player.position)<neutralHuman.fleeDistance)
        { 
            Vector3 a = neutralHuman.transform.position;
            Vector3 b = player.position;

            Vector3 dir = (a-b).normalized;
            Vector3 newPos =(dir* neutralHuman.fleeDistance); // sotre the new desired position
            Vector3 navNewPos; // store the new position in navmesh
            NavMeshHit hit;
            if(NavMesh.SamplePosition(newPos,out hit,1.0f,NavMesh.AllAreas))
            {
                navNewPos = hit.position;
                if (character.GetAgent().SetDestination(navNewPos))
                {
                    if (character.GetAgent().hasPath)
                    {
                        //dir = character.GetAgent().path.corners[1] - character.transform.position;
                        //Quaternion targetRotation = Quaternion.LookRotation(dir);
                        //// Smoothly rotate towards the target rotation
                        //character.transform.rotation = Quaternion.Slerp(character.transform.rotation, targetRotation, character.GetAgent().angularSpeed * Time.deltaTime);
                    }
                }
                else
                {
                    Debug.Log("Failed to set new destination");
                }
            }

            Quaternion targetRotation = Quaternion.LookRotation(dir);
            // Smoothly rotate towards the target rotation
            character.transform.rotation = Quaternion.Slerp(character.transform.rotation, targetRotation, character.GetAgent().angularSpeed * Time.deltaTime);

            if (neutralHuman.DebugCharacter)
             Debug.DrawRay(neutralHuman.transform.position, newPos, Color.blue);
        }
        else
        {
            controller.ChangeState(new NeutralHumanFear(neutralHuman, controller, "Terrified"));
        }
    }

    public override void StateUpdate()
    {
        base.StateUpdate(); 
    }

}
