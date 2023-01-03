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
        shouldCombo = false;
        base.OnEnter(machine);
        animator = machine.GetComponent<Animator>();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
      if(Input.GetButtonDown("Attack") && fixedtime > 0.1f)
            shouldCombo = true;

    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
