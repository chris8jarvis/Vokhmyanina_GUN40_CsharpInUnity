using UnityEngine;
using Zenject;
using Models;

namespace InteractiveObjects
{
    public class TrapSpring : MonoBehaviour
    {
        [Inject] private PlayerModel _playerModel;

        [Header("Spring Settings")]
        [SerializeField] private float _springForce = 15f;
        [SerializeField] private int _damage = 1;
        [SerializeField] private float _cooldown = 0.5f;

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
                    playerRb.AddForce(Vector3.up * _springForce, ForceMode.Impulse);
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