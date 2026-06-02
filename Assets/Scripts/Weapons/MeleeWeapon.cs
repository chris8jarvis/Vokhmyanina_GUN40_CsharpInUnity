using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    public float damage = 25f;
    public float attackRange = 1.8f;
    public float attackCooldown = 1f;
    public LayerMask targetLayer;
    
    private float lastAttackTime;
    private Animator animator;
    private Transform weaponTransform;
    private GameObject player;
    
    void Start()
    {
        animator = GetComponentInParent<Animator>();
        weaponTransform = transform;
        player = GameObject.FindGameObjectWithTag("Player");
        
        if (targetLayer == 0)
        {
            targetLayer = LayerMask.GetMask("Player");
        }
    }
    
    void Update()
    {
        if (player == null) return;
        
        float distance = Vector3.Distance(weaponTransform.position, player.transform.position);
        
        if (distance <= attackRange)
        {
            Attack();
        }
    }
    
    public void Attack()
    {
        if (Time.time < lastAttackTime + attackCooldown) return;
        
        lastAttackTime = Time.time;
        
        if (animator != null)
        {
            animator.SetTrigger("MeleeAttack");
        }
        
        Collider[] hits = Physics.OverlapSphere(weaponTransform.position, attackRange, targetLayer);
        
        foreach (Collider hit in hits)
        {
            Health health = hit.GetComponent<Health>();
            if (health != null)
            {
                Vector3 direction = (hit.transform.position - weaponTransform.position).normalized;
                health.TakeDamage(damage, direction);
                Debug.Log($"Нанесён урон {damage} цели {hit.name}");
            }
        }
    }
    
    void OnDrawGizmosSelected()
    {
        if (weaponTransform == null)
            weaponTransform = transform;
            
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(weaponTransform.position, attackRange);
    }
}