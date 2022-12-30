using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEntryState : State
{
    public override void OnEnter(StateMachine stateMachine)
    {
        base.OnEnter(stateMachine);

        if(stateMachine.player.canJump)
        {
            stateMachine.player.hit = true;
            stateMachine.player.rb.velocity = Vector2.zero;
            stateMachine.player.animator.SetBool("Moving", false);
            State nextState = (State)new GroundEntryState();
            stateMachine.SetNextState(nextState);
        } else
        {
            stateMachine.player.hit = true;
            stateMachine.player.rb.velocity = new Vector2(0, stateMachine.player.rb.velocity.y);
            stateMachine.player.animator.SetBool("isFalling", false);
            State nextState = (State)new AirAttackState();
            stateMachine.SetNextState(nextState);
        }
        
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
