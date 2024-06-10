using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterWander : BaseState
{
    Shooter shooter;
    public ShooterWander(Character character, CharacterStateMachine controller, string animationName) : base(character, controller, animationName)
    {
        shooter = (Shooter)character;
    }

    public override void Enter()
    {
        base.Enter();
        character.GetAgent().isStopped = true;
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
        if (shooter.isBitten)
        {
            controller.ChangeState(new ShooterTransform(shooter, controller, "Transform"));
            return;
        }
    }
}
