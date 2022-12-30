using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundExitState : MeleeBaseState
{
    public override void OnEnter(StateMachine stateMachine)
    {
        base.OnEnter(stateMachine);

        attackIndex = 4;
        duration = 1.5f;
        animator.SetTrigger("Attack " + attackIndex);
        Debug.Log("Attack " + attackIndex);
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
