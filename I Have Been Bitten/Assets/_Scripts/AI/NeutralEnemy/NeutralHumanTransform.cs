using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralHumanTransform : BaseState
{
    NeutralHuman human;
    public NeutralHumanTransform(Character neutralHuman, CharacterStateMachine controller,string animationName) : base(neutralHuman, controller,animationName)
    {
        human = (NeutralHuman)neutralHuman;   
    }

    public override void Enter()
    {
        base.Enter();   
        character.GetAnimator().SetTrigger(animationName);
        character.GetAudioSource().PlayOneShot(human.convertToZomby);
    }
    public override void Exit()
    {
        base.Exit();
       GameObject zombie = UnityEngine.Object.Instantiate(human.zombiePrefab);
       zombie.transform.position = human.transform.position;   
       zombie.transform.rotation = human.transform.rotation;
       zombie.transform.localScale = human.transform.localScale;

       UnityEngine.Object.Destroy(human.gameObject);
    }
    public override void CheckTransition()
    {
        base.CheckTransition();

        if (character.IsAnimationFinished())
        {
            Exit();
        }
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
    }
}
