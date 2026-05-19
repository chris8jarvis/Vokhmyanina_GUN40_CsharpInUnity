using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour
{
    [Header("Components")]
    public Animator animator;
    public NavMeshAgent agent;

    [Header("State Machine")]
    public IdleState idleState;
    public SearchState searchState;
    public CollectState collectState;
    
    private AIBaseState currentState;

    [Header("Settings")]
    public Transform collectableTarget;
    public float idleDuration = 5f;
    public float searchRadius = 5f; 

    void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (agent == null) agent = GetComponent<NavMeshAgent>();

        idleState = new IdleState(this);
        searchState = new SearchState(this);
        collectState = new CollectState(this);

        SwitchState(idleState);
    }

    void Update()
    {
        if (currentState != null)
            currentState.OnUpdate();
    }

    public void SwitchState(AIBaseState newState)
    {
        if (currentState != null)
            currentState.OnExit();
        
        currentState = newState;
        currentState.OnEnter();
    }

    public bool IsCollectableInRange()
    {
        if (collectableTarget == null) return false;
        return Vector3.Distance(transform.position, collectableTarget.position) <= searchRadius;
    }
}
