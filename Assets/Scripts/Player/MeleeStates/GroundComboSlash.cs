using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundComboSlash : MeleeBaseState
{
    public override void OnEnter(StateMachine stateMachine)
    {
        base.OnEnter(stateMachine);

        attackIndex = 3;
        duration = 0.58f;
        animator.SetTrigger("Attack " + attackIndex);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (fixedtime > duration)
        {
            if (shouldCombo)
                machine.SetNextState(new GroundExitState());
            else
                machine.SetNextStateToMain();
        }
    }
    public override void OnExit()
    {
        base.OnExit();

        machine.player.animator.SetBool("isDashing", false);
    }
}
