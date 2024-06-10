using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterTransform : BaseState
{
    Shooter shooter;
    public ShooterTransform(Character character, CharacterStateMachine controller, string animationName) : base(character, controller, animationName)
    {
        shooter = (Shooter)character;
    }
    public override void Enter()
    {
        base.Enter();
        shooter.GetAnimator().speed = 1;
        character.gameObject.layer = LayerMask.NameToLayer("Default");
        character.GetAudioSource().PlayOneShot(shooter.convertToZomby);
    }
    public override void Exit()
    {
        base.Exit();
        GameObject zombie = UnityEngine.Object.Instantiate(shooter.zombiePrefab);
        zombie.transform.position = shooter.transform.position;
        zombie.transform.rotation = shooter.transform.rotation;
        zombie.transform.localScale = shooter.transform.localScale;

        UnityEngine.Object.Destroy(shooter.gameObject);
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
