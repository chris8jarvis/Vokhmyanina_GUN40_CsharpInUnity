using UnityEngine;
using Zenject;
using Models;

namespace InteractiveObjects
{
    public class HealthBonus : MonoBehaviour
    {
        [Inject] private PlayerModel _playerModel;

        [Header("Animation Settings")]
        [SerializeField] private float _baseY = 0.1f;
        [SerializeField] private float _maxHeight = 1f;
        [SerializeField] private float _duration = 1f;

        [Header("Heal Settings")]
        [SerializeField] private int _healAmount = 1;

        private float _time;

        private void Update()
        {
            _time += Time.deltaTime;
            float progress = Mathf.PingPong(_time / _duration, 1f);
            float y = _baseY + (_maxHeight * progress);

            Vector3 pos = transform.position;
            pos.y = y;
            transform.position = pos;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _playerModel.Heal(_healAmount);
                Destroy(gameObject);
            }
        }
    }
}
