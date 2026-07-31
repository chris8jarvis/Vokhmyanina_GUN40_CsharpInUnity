using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace UI
{
    public class HealthView : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI _healthText;
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private GameObject _gameOverPanel;

        public void UpdateHealth(int health)
        {
            if (_healthText != null)
                _healthText.text = $"HEALTH: {health}";
        }

        public void UpdateScore(int score)
        {
            if (_scoreText != null)
                _scoreText.text = $"SCORE: {score}";
        }

        public void ShowGameOver(bool isGameOver)
        {
            if (_gameOverPanel != null)
                _gameOverPanel.SetActive(isGameOver);
        }
    }
}
