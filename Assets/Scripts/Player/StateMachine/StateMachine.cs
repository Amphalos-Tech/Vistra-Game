using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public string nameCustom;
    public State CurrentState;
    public Player player;

    void Start()
    {
        player = GetComponent<Player>();
        if (nameCustom == "MeleeMC")
            CurrentState = (State) new Idle();
        if (CurrentState != null)
            CurrentState.OnEnter(this);
    }

    private void OnEnable()
    {
        if (nameCustom == "MeleeMC")
            CurrentState = (State)new Idle();
        if (CurrentState != null)
            CurrentState.OnEnter(this);
    }
    // Update is called once per frame
    void Update()
    {
        if (CurrentState != null)
        {
            CurrentState.OnUpdate();
        }
    }

    public void SetNextState(State nextState)
    {
        if(CurrentState != null)
            CurrentState.OnExit();
        CurrentState = nextState;
        CurrentState.OnEnter(this);

    }

    public void SetNextStateToMain()
    {
        player.hit = false;
        CurrentState = new Idle();
    }
}
