using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEntryState : MeleeBaseState
{
    public override void OnEnter(StateMachine stateMachine)
    {
        base.OnEnter(stateMachine);

        attackIndex = 1;
        duration = 0.5f;
        fixedtime = 0f;
        animator.SetTrigger("Attack " + attackIndex);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        //machine.player.hit = true;
        if (fixedtime >= duration)
        {
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
