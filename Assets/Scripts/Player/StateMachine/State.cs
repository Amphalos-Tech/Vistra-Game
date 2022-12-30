using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    protected StateMachine machine;
    protected float fixedtime;

    public virtual void OnEnter(StateMachine stateMachine)
    {
        machine = stateMachine;
    }

    public virtual void OnUpdate() 
    {
        fixedtime += UnityEngine.Time.deltaTime;
    }

    public virtual void OnExit() { }
}
