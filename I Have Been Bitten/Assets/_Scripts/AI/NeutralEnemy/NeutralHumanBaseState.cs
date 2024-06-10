using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class NeutralHumanBaseState 
{
    protected NeutralHuman neutralHuman;
    protected CharacterStateMachine controller;
    protected string animationName;

    protected bool isExitingState;
    protected bool isAnimationFinished;
    protected float startTime;
    public NeutralHumanBaseState(NeutralHuman neutralHuman, CharacterStateMachine controller, string animationName)
    {
        this.neutralHuman = neutralHuman;
        this.controller = controller;
        this.animationName = animationName;
    }

    public virtual void Enter()
    {
        isAnimationFinished = false;
        isExitingState = false;
        startTime = Time.time;
        neutralHuman.GetAnimator().SetBool(animationName, true);
    }

    public virtual void Exit()
    {
        isExitingState = true;
        if (!isAnimationFinished) isAnimationFinished = true;
        neutralHuman.GetAnimator().SetBool(animationName, false);
    }
    public virtual void CheckTransition()
    {

    }

    public virtual void StateUpdate()
    {
        CheckTransition();
    }
}
