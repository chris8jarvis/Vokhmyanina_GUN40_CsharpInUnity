using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    [SerializeField] private Transform _target; // игрок, за которым следит камера
    [SerializeField] private Vector3 _offset;   // смещение камеры относительно игрока

    private void LateUpdate()
    {
        if (_target == null)
        {
            Debug.LogWarning("Target not assigned to MiniMap script!");
            return;
        }

        Vector3 targetPosition = _target.position + _offset;
        targetPosition.y = transform.position.y;

        transform.position = targetPosition;
    }
}
