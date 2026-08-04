using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace UI
{
    public class RadarView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform _container; // PointsContainer
        [SerializeField] private GameObject _playerDotPrefab;
        [SerializeField] private GameObject _bonusDotPrefab;
        [SerializeField] private GameObject _trapDotPrefab;

        [Header("Settings")]
        [SerializeField] private float _radius = 60f;

        private GameObject _currentPlayerDot;
        private List<GameObject> _bonusDots = new List<GameObject>();
        private List<GameObject> _trapDots = new List<GameObject>();

        public void UpdateRadar(Vector3 playerPosition, List<Transform> bonuses, List<Transform> traps)
        {
            if (_currentPlayerDot == null)
            {
                _currentPlayerDot = Instantiate(_playerDotPrefab, _container);
                (_currentPlayerDot.transform as RectTransform).anchoredPosition = Vector2.zero;
            }

            UpdateDotList(bonuses, _bonusDots, _bonusDotPrefab, playerPosition, Color.green);

            UpdateDotList(traps, _trapDots, _trapDotPrefab, playerPosition, Color.red);
        }

        private void UpdateDotList(List<Transform> objects, List<GameObject> dots, GameObject prefab, Vector3 playerPos, Color color)
        {
            foreach (var dot in dots)
                Destroy(dot);
            dots.Clear();

            foreach (var obj in objects)
            {
                if (obj == null) continue;

                Vector3 relative = obj.position - playerPos;
                float distance = relative.magnitude;

                if (distance > 50f) continue;

                Vector2 radarPos = new Vector2(relative.x, relative.z) / distance * Mathf.Min(distance, _radius);

                GameObject dot = Instantiate(prefab, _container);
                dot.GetComponent<Image>().color = color;
                (dot.transform as RectTransform).anchoredPosition = radarPos;
                dots.Add(dot);
            }
        }
    }
}