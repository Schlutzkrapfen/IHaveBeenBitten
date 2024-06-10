using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState
{
    protected Character character;
    protected CharacterStateMachine controller;
    protected string animationName;

    protected bool isExitingState;
    protected bool isAnimationFinished;
    protected float startTime;
    public BaseState(Character character, CharacterStateMachine controller, string animationName)
    {
        this.character = character;
        this.controller = controller;
        this.animationName = animationName;
    }

    public virtual void Enter()
    {
        isAnimationFinished = false;
        isExitingState = false;
        startTime = Time.time;
        character.GetAnimator().SetBool(animationName, true);
        character.GetAnimator().Play(character.GetAnimator().GetCurrentAnimatorStateInfo(0).fullPathHash, 0, 0.0f);
    }

    public virtual void Exit()
    {
        isExitingState = true;
        if (!isAnimationFinished) isAnimationFinished = true;
        character.GetAnimator().SetBool(animationName, false);

        character.GetAudioSource().Stop();
    }
    public virtual void CheckTransition()
    {

    }

    public virtual void StateUpdate()
    {
        CheckTransition();
    }
}
