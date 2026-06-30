using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiChasePlayerState : AiState
{
    
    float timer = 0.0f;

    public AiStateId GetId() {
        return AiStateId.ChasePlayer;
    }

    public void Enter(AiAgent agent) 
    {
        agent.navMeshAgent.stoppingDistance = 0f;
        agent.navMeshAgent.speed = agent.config.findTargetSpeed;
    }

    public void Update(AiAgent agent) 
    {
         if (!agent.targeting.HasTarget)
        {
            agent.stateMachine.ChangeState(AiStateId.FindTarget);
            return;
        }

        agent.navMeshAgent.destination = agent.targeting.TargetPosition;

        float distance = agent.targeting.TargetDistance;
        if (distance <= agent.config.attackStoppingDistance)
        {
            agent.stateMachine.ChangeState(AiStateId.MeleeAttackTarget);
        }
    }

    public void Exit(AiAgent agent)
    {
        agent.navMeshAgent.stoppingDistance = 0f;
    }
}
