using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectState : AIBaseState
{
    private float collectDistance = 1.5f;
    private bool hasCollected = false;
    
    public CollectState(AIAgent agent) : base(agent) { }
    
    public override void OnEnter()
    {
        hasCollected = false;
        agent.animator.SetTrigger("toCollect");
        Debug.Log("Entered collect state");
    }
    
    public override void OnUpdate()
    {
        if (hasCollected) return;
        
        if (agent.collectableTarget == null)
        {
            agent.SwitchState(agent.idleState);
            return;
        }
        
        agent.agent.SetDestination(agent.collectableTarget.position);
        
        float distance = Vector3.Distance(agent.transform.position, agent.collectableTarget.position);
        
        if (distance <= collectDistance)
        {
            CollectItem();
        }
    }
    
    private void CollectItem()
    {
        hasCollected = true;
        Debug.Log("Item collected!");
        
        if (agent.collectableTarget != null)
        {
            Object.Destroy(agent.collectableTarget.gameObject);
            agent.collectableTarget = null;
        }
        agent.SwitchState(agent.idleState);
    }
    
    public override void OnExit()
    {
        Debug.Log("Exited collect state");
    }
}
