using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiMeleeAttackState : AiState
{
    private float lastAttackTime;
    
    public AiStateId GetId()
    {
        return AiStateId.MeleeAttackTarget;
    }
    
    public void Enter(AiAgent agent)
    {
        agent.navMeshAgent.stoppingDistance = agent.config.attackStoppingDistance;
        agent.navMeshAgent.speed = agent.config.attackSpeed;
        lastAttackTime = -999f;
    }
    
    public void Update(AiAgent agent)
    {
        // Если нет цели, ищем
        if (!agent.targeting.HasTarget)
        {
            agent.stateMachine.ChangeState(AiStateId.FindTarget);
            return;
        }
        
        // Двигаемся к цели
        agent.navMeshAgent.destination = agent.targeting.TargetPosition;
        
        // Проверяем расстояние до цели
        float distance = agent.targeting.TargetDistance;
        
        // Если цель в радиусе атаки, атакуем
        if (distance <= agent.config.attackStoppingDistance)
        {
            TryMeleeAttack(agent);
        }
        
        // Если здоровье низкое, то ищем аптечку
        if (agent.health.IsLowHealth())
        {
            agent.stateMachine.ChangeState(AiStateId.FindHealth);
        }
    }
    
    private void TryMeleeAttack(AiAgent agent)
    {
        float cooldown = 1f;
        if (Time.time < lastAttackTime + cooldown) return;
        
        // Ищем MeleeWeapon на враге или его детях
        MeleeWeapon meleeWeapon = agent.GetComponentInChildren<MeleeWeapon>();
        if (meleeWeapon != null)
        {
            meleeWeapon.Attack();
            lastAttackTime = Time.time;
            Debug.Log($"{agent.name} атакует ближним боем");
        }
        else
        {
            Debug.LogWarning($"У {agent.name} нет компонента MeleeWeapon");
        }
    }
    
    public void Exit(AiAgent agent)
    {
        // Сбрасываем настройки при выходе из состояния
        agent.navMeshAgent.stoppingDistance = 0f;
    }
}
