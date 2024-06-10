using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralHumanFear : BaseState
{
    NeutralHuman neutralHuman;
    Transform player;
    public NeutralHumanFear(Character character, CharacterStateMachine controller, string animationName) : base(character, controller, animationName)
    {
        neutralHuman = (NeutralHuman)character;
        player = GameObject.FindObjectOfType<FirstPersonController>().transform;
    }
    public override void Enter()
    {
        base.Enter();
        neutralHuman.GetAgent().isStopped = true;
        character.GetAnimator().SetTrigger(animationName);
        character.GetAudioSource().clip = neutralHuman.fearAudioClip;
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

        if (neutralHuman.isBitten)
        {
            controller.ChangeState(new NeutralHumanTransform(neutralHuman, controller, "Transform"));
        }
        if (Vector3.Distance(neutralHuman.transform.position, player.position) < (neutralHuman.fleeDistance-2) && neutralHuman.ShouldRun)
        {
            controller.ChangeState(new NeutralHumanWander(neutralHuman, controller, "Wander"));
        }
    }
}
