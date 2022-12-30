using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeBaseState : State
{
    public float duration;

    protected Animator animator;
    protected int attackIndex;
    protected bool shouldCombo;

    public override void OnEnter(StateMachine machine)
    {
        base.OnEnter(machine);
        animator = machine.GetComponent<Animator>();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

      if(Input.GetButtonDown("Attack"))
            shouldCombo = true;
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
