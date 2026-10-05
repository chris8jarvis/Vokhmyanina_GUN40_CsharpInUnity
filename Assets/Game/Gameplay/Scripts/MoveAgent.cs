using UnityEngine;
using UnityEngine.AI;

namespace SampleProject
{
    public sealed class MoveAgent : MonoBehaviour
    {
        private const float STOPPING_DISTANCE_SQR = 0.2f;
        private const float ROTATION_SPEED = 10f;

        [Header("Move Speed")]
        [SerializeField]
        private float moveSpeed = 5f;

        private NavMeshPath navMeshPath;
        private Vector3[] pointPath;
        private int pointer;
        private bool isMoving;

        public bool IsCompleted => !this.isMoving;

        private void Awake()
        {
            this.navMeshPath = new NavMeshPath();
        }

        public void SetDestination(Vector3 destination)
        {
            if (NavMesh.SamplePosition(destination, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                destination = hit.position;
            }
            else
            {
                return;
            }

            Vector3 startPosition = this.transform.position;
            if (NavMesh.SamplePosition(startPosition, out NavMeshHit startHit, 2f, NavMesh.AllAreas))
            {
                startPosition = startHit.position;
            }

            if (!NavMesh.CalculatePath(startPosition, destination, NavMesh.AllAreas, this.navMeshPath))
            {
                return;
            }

            if (this.navMeshPath.corners.Length < 2)
            {
                this.isMoving = false;
                return;
            }

            this.pointPath = this.navMeshPath.corners;
            this.pointer = 1;
            this.isMoving = true;
        }

        private void Update()
        {
            if (!this.isMoving || this.pointPath == null || this.pointer >= this.pointPath.Length)
            {
                return;
            }

            Vector3 current = this.transform.position;
            Vector3 target = this.pointPath[this.pointer];
            Vector3 direction = target - current;

            if (direction.sqrMagnitude <= STOPPING_DISTANCE_SQR)
            {
                this.pointer++;
                if (this.pointer >= this.pointPath.Length)
                {
                    this.isMoving = false;
                }
                return;
            }

            this.transform.position = Vector3.MoveTowards(
                current,
                target,
                this.moveSpeed * Time.deltaTime
            );

            Vector3 flatDir = new Vector3(direction.x, 0f, direction.z);
            if (flatDir.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(flatDir);
                this.transform.rotation = Quaternion.Slerp(
                    this.transform.rotation,
                    targetRotation,
                    ROTATION_SPEED * Time.deltaTime
                );
            }
        }
    }
}