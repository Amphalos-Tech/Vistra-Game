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
      if (Input.GetButtonDown("Dash") && attackIndex != 4 && machine.player.dashCooldownCount <= 0 && machine.player.moveDirection.x != 0)
      {
            if (machine.player.transform.localRotation == Quaternion.Euler(0f, 0f, 0f) && machine.player.moveDirection.x < 0)
                machine.player.transform.localRotation = Quaternion.Euler(0, 180, 0);
            else if (machine.player.transform.localRotation == Quaternion.Euler(0f, 180f, 0f) && machine.player.moveDirection.x > 0)
                machine.player.transform.localRotation = Quaternion.Euler(0, 0, 0);
          machine.player.dashCooldownCount = machine.player.dashCooldown;
            if (attackIndex == 1)
            {
                machine.player.cancelled = true;
                machine.player.dashSpeed /= 2.5f;
            }
          animator.SetTrigger("DashCancel");
          machine.SetNextStateToMain();
          animator.SetBool("isDashing", true);
      }
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
