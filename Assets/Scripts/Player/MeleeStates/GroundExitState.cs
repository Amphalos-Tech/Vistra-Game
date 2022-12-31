using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundExitState : MeleeBaseState
{
    public override void OnEnter(StateMachine stateMachine)
    {
        base.OnEnter(stateMachine);

        attackIndex = 4;
        duration = 1.25f;
        stateMachine.player.hit = false;
        stateMachine.player.canJump = false;
        animator.SetTrigger("Attack " + attackIndex);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        machine.player.animator.SetBool("isDashing", false);
        if (fixedtime > duration)
        {
            machine.player.canJump = true;
            machine.SetNextStateToMain();
        }
    }
    public override void OnExit()
    {
        base.OnExit();
    }
}
