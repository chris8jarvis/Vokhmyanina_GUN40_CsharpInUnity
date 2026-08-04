using UnityEngine;
using Zenject;
using Models;

namespace InteractiveObjects
{
    public class SpeedBonus : MonoBehaviour
    {
        [Inject] private PlayerModel _playerModel;

        [Header("Speed Settings")]
        [SerializeField] private float _speedMultiplier = 2f; 
        [SerializeField] private float _duration = 3f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
                if (playerMovement != null)
                {
                    playerMovement.ApplySpeedBoost(_speedMultiplier, _duration);
                }
                else
                {
                    Debug.LogWarning("[SpeedBonus] PlayerMovement component not found");
                }

                Destroy(gameObject);
            }
        }
    }
}
