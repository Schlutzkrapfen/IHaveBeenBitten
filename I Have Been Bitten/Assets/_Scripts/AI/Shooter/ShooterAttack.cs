using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterAttack : BaseState
{
    Shooter shooter;
    string[] shootTargets = new string[3];
    public ShooterAttack(Character character, CharacterStateMachine controller, string animationName) : base(character, controller, animationName)
    {
        shooter = (Shooter)character;

        shootTargets[0] = "Neck";
        shootTargets[1] = "Hips";
        shootTargets[2] = "Spine_02";
    }

    public override void Enter()
    {
        base.Enter();
        character.GetAgent().isStopped = true;
        int i = Random.Range(0, shootTargets.Length);

        Transform target = shooter.AimToTarget(character.GetTarget(), "Neck") ;

        if (target != null)
        {
            shooter.SetAimContrains(target);
            shooter.SetAimWeigthRig(1f);
            shooter.ResetRigLerpTimer();
        }
        else
        {
            if (character.DebugCharacter) Debug.Log("Couldnt find taget to shoot");
        }        
    }

    public override void Exit()
    {
        base.Exit();
        shooter.SetAimWeigthRig(0f);
        shooter.ResetRigLerpTimer();
        shooter.ClearAimConstrains();
    }
    public override void CheckTransition()
    {
        base.CheckTransition();
    }
    public override void StateUpdate()
    {
        base.StateUpdate();
        

        Transform target = character.GetTarget();
        if (target == null)
        {
            shooter.ClearAimConstrains();
            shooter.GetAnimator().speed = 1;
            if (character.IsAnimationFinished())
            {
                controller.ChangeState(new ShooterIdle(character, controller, "Idle")); 
            }
            return;
        }

        if (shooter.isBitten)
        {
            controller.ChangeState(new ShooterTransform(shooter, controller, "Transform"));
            return;
        }

        ////this should be in a corroutine
        if (shooter.CheckConstrains() == false)
        {
            shooter.SetAimContrains(shooter.AimToTarget(target, "Neck"));
        }

        //check if the fire rate, if less than 0 animate the shoot anim, and when the shoot is trigerred the timer will restart
        if (shooter.GetFirerateTimer() <= 0 ||shooter.InstaShoot)
        {
            shooter.GetAnimator().speed = 1;
            shooter.InstaShoot = false;
        }
        else
        {
            shooter.GetAnimator().speed = 0;
            character.GetAnimator().Play(animationName, 0, 0.1f);
        }

        Vector3 dir = target.transform.position - character.transform.position;
        dir.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(dir);

        // Smoothly rotate towards the target rotation
        character.transform.rotation = Quaternion.Slerp(character.transform.rotation, targetRotation, character.GetAgent().angularSpeed * Time.deltaTime);

    }
}
