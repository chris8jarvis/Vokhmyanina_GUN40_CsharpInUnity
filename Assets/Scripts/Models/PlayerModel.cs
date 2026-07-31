using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Models
{
    public class PlayerModel
    {
        private readonly ReactiveProperty<int> _health = new ReactiveProperty<int>(3);
        private readonly ReactiveProperty<int> _score = new ReactiveProperty<int>(0);
        private readonly ReactiveProperty<bool> _isGameOver = new ReactiveProperty<bool>(false);

        public IReadOnlyReactiveProperty<int> Health => _health;
        public IReadOnlyReactiveProperty<int> Score => _score;
        public IReadOnlyReactiveProperty<bool> IsGameOver => _isGameOver;

        public void Heal(int amount)
        {
            if (amount <= 0) return;
            _health.Value += amount;
            Debug.Log($"[PlayerModel] Healed: +{amount}, Health: {_health.Value}");
        }

        public void TakeDamage(int amount)
        {
            if (amount <= 0) return;
            _health.Value -= amount;
            Debug.Log($"[PlayerModel] Damaged: -{amount}, Health: {_health.Value}");

            if (_health.Value <= 0)
            {
                _health.Value = 0;
                _isGameOver.Value = true;
                Debug.Log("[PlayerModel] Game Over");
            }
        }

        public void AddScore(int amount)
        {
            if (amount <= 0) return;
            _score.Value += amount;
            Debug.Log($"[PlayerModel] Score: +{amount}, Total: {_score.Value}");
        }

        public void ResetGame()
        {
            _health.Value = 3;
            _score.Value = 0;
            _isGameOver.Value = false;
            Debug.Log("[PlayerModel] Game reset");
        }
    }
}
