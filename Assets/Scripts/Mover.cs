using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Mover : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private Vector3 _startOffset;  // ← Смещение от текущей позиции
    [SerializeField] private Vector3 _endOffset;    // ← Смещение от текущей позиции
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _delay = 1f;
    
    private Vector3 _start;
    private Vector3 _end;
    private Rigidbody _rb;
    
    void OnDrawGizmos()
    {
        // Рассчитываем абсолютные позиции на основе текущей позиции объекта
        Vector3 startPos = transform.position + _startOffset;
        Vector3 endPos = transform.position + _endOffset;
        
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(startPos, 0.5f);
        
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(endPos, 0.5f);
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(startPos, endPos);
        
        // Подписи
        #if UNITY_EDITOR
        Handles.Label(startPos + Vector3.up * 0.5f, "Start");
        Handles.Label(endPos + Vector3.up * 0.5f, "End");
        #endif
    }
    
    void Start()
    {
        // Рассчитываем абсолютные позиции на старте
        _start = transform.position + _startOffset;
        _end = transform.position + _endOffset;
        
        StartCoroutine(MoveRoutine());
    }
    
    IEnumerator MoveRoutine()
    {
        _rb = GetComponent<Rigidbody>();
        if (_rb == null) yield break;
        _rb.isKinematic = true;
        
        while (true)
        {
            yield return StartCoroutine(MoveToPosition(_start, _end));
            yield return new WaitForSeconds(_delay);
            yield return StartCoroutine(MoveToPosition(_end, _start));
            yield return new WaitForSeconds(_delay);
        }
    }
    
    IEnumerator MoveToPosition(Vector3 from, Vector3 to)
    {
        float distance = Vector3.Distance(from, to);
        float duration = distance / _speed;
        float elapsedTime = 0f;
        
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            Vector3 newPosition = Vector3.Lerp(from, to, t);
            _rb.MovePosition(newPosition);
            yield return null;
        }
    }
}
