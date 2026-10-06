using Game.GameEngine.Ecs;
using UnityEngine;

namespace SampleProject
{
    [RequireComponent(typeof(MoveAgent))]
    [RequireComponent(typeof(Entity))]
    public sealed class EnemyDetector : MonoBehaviour
    {
        [Header("Vision Radius")]
        [SerializeField]
        private float visionRadius = 8f;

        [Header("Attack distance")]
        [SerializeField]
        private float attackRange = 1.5f;

        [Header("Enemy search period")]
        [SerializeField]
        private float searchPeriod = 0.3f;

        [Header("Enemy search masks")]
        [SerializeField]
        private LayerMask enemyMask = ~0;

        private MoveAgent moveAgent;
        private Entity selfEntity;
        private Transform currentEnemy;
        private float searchTimer;
        private bool isAttacking;

        private void Awake()
        {
            this.moveAgent = this.GetComponent<MoveAgent>();
            this.selfEntity = this.GetComponent<Entity>();
        }

        private void Update()
        {
            this.searchTimer -= Time.deltaTime;
            if (this.searchTimer <= 0f)
            {
                this.searchTimer = this.searchPeriod;
                this.FindEnemy();
            }

            if (this.currentEnemy == null)
            {
                this.isAttacking = false;
                return;
            }

            if (!this.currentEnemy.gameObject.activeInHierarchy)
            {
                this.currentEnemy = null;
                this.isAttacking = false;
                return;
            }

            float dist = Vector3.Distance(this.transform.position, this.currentEnemy.position);

            if (dist > this.attackRange)
            {
                if (this.isAttacking)
                {
                    this.selfEntity.RemoveData<CommandRequest>();
                    this.isAttacking = false;
                }

                this.moveAgent.SetDestination(this.currentEnemy.position);
            }
            else
            {
                if (!this.isAttacking)
                {
                    this.isAttacking = true;
                    this.Attack();
                }
            }
        }

        private void FindEnemy()
        {
            Collider[] hits = Physics.OverlapSphere(
                this.transform.position,
                this.visionRadius,
                this.enemyMask,
                QueryTriggerInteraction.Ignore
            );

            Transform closest = null;
            float minDist = float.MaxValue;

            foreach (var hit in hits)
            {
                if (!hit.CompareTag("Enemy")) continue;

                float d = Vector3.Distance(this.transform.position, hit.transform.position);
                if (d < minDist)
                {
                    minDist = d;
                    closest = hit.transform;
                }
            }

            this.currentEnemy = closest;
        }

        private void Attack()
        {
            if (this.currentEnemy == null) return;

            var enemyEntity = this.currentEnemy.GetComponentInParent<Entity>();
            if (enemyEntity == null)
            {
                Debug.LogWarning($"[EnemyDetector] У {this.currentEnemy.name} нет Entity.");
                return;
            }

            this.selfEntity.SetData(new CommandRequest
            {
                type = CommandType.ATTACK_TARGET,
                args = enemyEntity,
                status = CommandStatus.IDLE
            });

            Debug.Log($"[EnemyDetector] {this.name} атакует {this.currentEnemy.name}.");
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1f, 1f, 0f, 0.2f);
            Gizmos.DrawSphere(this.transform.position, this.visionRadius);

            Gizmos.color = new Color(1f, 0f, 0f, 0.4f);
            Gizmos.DrawSphere(this.transform.position, this.attackRange);
        }
#endif
    }
}
