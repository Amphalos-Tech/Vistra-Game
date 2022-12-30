using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundComboSpin : MeleeBaseState
{
    public override void OnEnter(StateMachine stateMachine)
    {
        base.OnEnter(stateMachine);

        attackIndex = 2;
        duration = 0.6f;
        animator.SetTrigger("Attack " + attackIndex);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (fixedtime > duration)
        {
            if (shouldCombo)
                machine.SetNextState(new GroundComboSlash());
            else
                machine.SetNextStateToMain();
        }
    }
    public override void OnExit()
    {
        base.OnExit();
    }
}
