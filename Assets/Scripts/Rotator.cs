using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour
{
    [SerializeField] private Vector3 _rotate = new Vector3(0, 45, 0);
    
    private Rigidbody _rb;
    
    IEnumerator Start()
    {
        // 1. Находим физическое тело объекта
        _rb = GetComponent<Rigidbody>();
        
        if (_rb == null)
        {
            Debug.LogError("Rotator: No Rigidbody found!");
            yield break;
        }
        
        // 2. Делаем его кинематическим
        _rb.isKinematic = true;
        
        Debug.Log(gameObject.name + ": Rotator started with speed " + _rotate);
        
        // 3. Бесконечный цикл
        while (true)
        {
            // 4. Вращаем кинематическое тело каждый кадр
            Quaternion deltaRotation = Quaternion.Euler(_rotate * Time.deltaTime);
            _rb.MoveRotation(_rb.rotation * deltaRotation);
            
            // 5. Ждём следующий кадр
            yield return null;
        }
    }
}