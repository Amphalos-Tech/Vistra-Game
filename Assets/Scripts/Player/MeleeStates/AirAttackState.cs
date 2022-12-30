using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirAttackState : MeleeBaseState
{
    public override void OnEnter(StateMachine stateMachine)
    {
        base.OnEnter(stateMachine);

        attackIndex = 5;
        duration = 0.3f;
        fixedtime = 0f;
        animator.SetTrigger("Attack " + attackIndex);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (fixedtime > duration)
        {
            machine.SetNextStateToMain();
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
