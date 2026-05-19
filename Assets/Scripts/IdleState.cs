using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : AIBaseState
{
    private float idleTimer = 0f;
    
    public IdleState(AIAgent agent) : base(agent) { }
    
    public override void OnEnter()
    {
        idleTimer = 0f;
        agent.animator.SetTrigger("toIdle");
        agent.agent.ResetPath(); 
        Debug.Log("Entered idle state");
    }
    
    public override void OnUpdate()
    {
        idleTimer += Time.deltaTime;
        
        if (idleTimer >= agent.idleDuration)
        {
            agent.SwitchState(agent.searchState);
        }
    }
    
    public override void OnExit()
    {
        Debug.Log("Exited idle state");
    }
}
