using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset = new Vector3(0, 5, -8);

    private void LateUpdate()
    {
        if (_target == null) return;
        transform.position = _target.position + _offset;
        transform.LookAt(_target);
    }
}
