using System.Collections.Generic;
using UnityEngine;

namespace SampleProject
{
    public sealed class ObjectsMover : MonoBehaviour
    {
        [Header("Movement Manager")]
        [SerializeField]
        private MovingGroupManager movingGroupManager;

        [Header("Camera")]
        [SerializeField]
        private Camera targetCamera;

        private readonly List<MoveAgent> selectedAgents = new();

        private void Update()
        {
            // ЛКМ — выделение (с Shift для добавления)
            if (Input.GetMouseButtonDown(0))
            {
                this.TrySelect();
            }

            // ПКМ — команда движения
            if (Input.GetMouseButtonDown(1))
            {
                this.MoveSelected();
            }
        }

        private void TrySelect()
        {
            Ray ray = this.targetCamera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                return;
            }

            var agent = hit.collider.GetComponentInParent<MoveAgent>();
            if (agent == null)
            {
                // Клик по пустому месту — сбросить выделение
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    this.ClearSelection();
                }
                return;
            }

            bool isShift = Input.GetKey(KeyCode.LeftShift);
            if (!isShift)
            {
                this.ClearSelection();
            }

            if (!this.selectedAgents.Contains(agent))
            {
                this.selectedAgents.Add(agent);
            }
        }

        private void MoveSelected()
        {
            if (this.selectedAgents.Count == 0)
            {
                return;
            }

            Ray ray = this.targetCamera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                return;
            }

            this.movingGroupManager.SendGroup(this.selectedAgents, hit.point);
        }

        private void ClearSelection()
        {
            this.selectedAgents.Clear();
        }
    }
}
