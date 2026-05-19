using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SearchState : AIBaseState
{
    private float randomMoveTimer = 0f;
    private float moveInterval = 2f;
    
    public SearchState(AIAgent agent) : base(agent) { }
    
    public override void OnEnter()
    {
        agent.animator.SetTrigger("toSearch");
        Debug.Log("Entered search state");
    }
    
    public override void OnUpdate()
    {
        if (agent.IsCollectableInRange())
        {
            agent.SwitchState(agent.collectState);
            return;
        }
        randomMoveTimer += Time.deltaTime;
        if (randomMoveTimer >= moveInterval)
        {
            SetRandomDestination();
            randomMoveTimer = 0f;
        }
    }
    
    private void SetRandomDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * 10f;
        randomDirection += agent.transform.position;
        
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, 10f, NavMesh.AllAreas))
        {
            agent.agent.SetDestination(hit.position);
        }
    }
    
    public override void OnExit()
    {
        Debug.Log("Exited search state");
    }
}
