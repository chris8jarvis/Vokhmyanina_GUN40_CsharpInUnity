using UnityEngine;
using UnityEngine.Events;

public class Pin : MonoBehaviour
{
    public UnityEvent onPinFall;
    private bool isFallen = false;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    void Update()
    {
        if (!isFallen && transform.up.y < 0.7f)
        {
            isFallen = true;
            onPinFall?.Invoke();
        }
    }

    void CheckFall()
    {
        if (transform.up.y < 0.8f && !isFallen)
        {
            isFallen = true;
            onPinFall?.Invoke();
        }
    }

    public void ResetPin()
    {
        isFallen = false;
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
