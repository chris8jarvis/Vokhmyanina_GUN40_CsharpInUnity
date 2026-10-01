using UnityEngine;

namespace SampleProject
{
    // Временный помощник для теста патруля.
    // Заполняет массив точек в CommandController и вызывает Patrol.
    public sealed class PatrolAutoTester : MonoBehaviour
    {
        [Header("Контроллер, которому отдаём команду")]
        [SerializeField]
        private CommandController controller;

        [Header("5 точек патруля")]
        [SerializeField]
        private Transform[] points = new Transform[5];

        [Header("Вызвать Patrol при старте?")]
        [SerializeField]
        private bool invokeOnStart = true;

        private void Start()
        {
            if (!this.invokeOnStart) return;
            this.InvokePatrol();
        }

        [ContextMenu("Invoke Patrol Now")]
        public void InvokePatrol()
        {
            if (this.controller == null)
            {
                Debug.LogError("[PatrolAutoTester] Не назначен CommandController.");
                return;
            }

            if (this.points == null || this.points.Length == 0)
            {
                Debug.LogError("[PatrolAutoTester] Массив points пуст.");
                return;
            }

            for (int i = 0; i < this.points.Length; i++)
            {
                if (this.points[i] == null)
                {
                    Debug.LogError($"[PatrolAutoTester] points[{i}] == null.");
                    return;
                }
            }

            this.controller.Patrol(this.points);
            Debug.Log($"[PatrolAutoTester] Отправлена команда Patrol на {this.points.Length} точек.");
        }
    }
}
