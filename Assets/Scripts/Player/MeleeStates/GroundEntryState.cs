using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEntryState : MeleeBaseState
{
    public override void OnEnter(StateMachine stateMachine)
    {
        base.OnEnter(stateMachine);

        attackIndex = 1;
        duration = 0.4f;
        fixedtime = 0f;
        animator.SetTrigger("Attack " + attackIndex);
        Debug.Log("Attack " + attackIndex);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if(fixedtime >= duration)
        {
            Debug.Log("g");
            if (shouldCombo)
                machine.SetNextState(new GroundComboSpin());
            else
                machine.SetNextStateToMain();
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
