using UnityEngine;
using Zenject;
using Models;

namespace InteractiveObjects
{
    public class TrapDoor : MonoBehaviour
    {
        [Inject] private PlayerModel _playerModel;

        [Header("Trap Settings")]
        [SerializeField] private int _damage = 1;
        [SerializeField] private float _pushForce = 10f;
        [SerializeField] private float _cooldown = 1f;

        private bool _isReady = true;

        private void OnCollisionEnter(Collision collision)
        {
            if (!_isReady) return;

            if (collision.gameObject.CompareTag("Player"))
            {
                _playerModel.TakeDamage(_damage);

                Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
                if (playerRb != null)
                {
                    Vector3 pushDirection = (collision.transform.position - transform.position).normalized;
                    pushDirection.y = 0.5f;
                    playerRb.AddForce(pushDirection * _pushForce, ForceMode.Impulse);
                }

                _isReady = false;
                Invoke(nameof(ResetCooldown), _cooldown);
            }
        }

        private void ResetCooldown()
        {
            _isReady = true;
        }
    }
}
