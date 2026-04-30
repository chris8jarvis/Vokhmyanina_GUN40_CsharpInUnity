using UnityEngine;

public class HooverAutomatic : MonoBehaviour
{
    [Header("Настройки движения")]
    public float speed = 3f;
    public float rotationSpeed = 100f;
    
    [Header("Настройки Raycast")]
    public float rayDistance = 1.5f;
    public LayerMask obstacleLayer;
    
    private Rigidbody rb;
    private bool isAvoiding = false;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }
    
    void FixedUpdate()
    {
        AvoidObstacles();
        
        MoveForward();
    }
    
    void AvoidObstacles()
    {
        bool frontBlocked = false;
        bool leftFree = false;
        bool rightFree = false;
        
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up * 0.5f;
        
        if (Physics.Raycast(rayOrigin, transform.forward, out hit, rayDistance, obstacleLayer))
        {
            frontBlocked = true;
            Debug.DrawRay(rayOrigin, transform.forward * rayDistance, Color.red);
        }
        else
        {
            Debug.DrawRay(rayOrigin, transform.forward * rayDistance, Color.green);
        }
        
        if (!Physics.Raycast(rayOrigin, -transform.right, out hit, rayDistance, obstacleLayer))
        {
            leftFree = true;
            Debug.DrawRay(rayOrigin, -transform.right * rayDistance, Color.green);
        }
        else
        {
            Debug.DrawRay(rayOrigin, -transform.right * rayDistance, Color.red);
        }
        
        if (!Physics.Raycast(rayOrigin, transform.right, out hit, rayDistance, obstacleLayer))
        {
            rightFree = true;
            Debug.DrawRay(rayOrigin, transform.right * rayDistance, Color.green);
        }
        else
        {
            Debug.DrawRay(rayOrigin, transform.right * rayDistance, Color.red);
        }
        
        if (frontBlocked)
        {
            if (leftFree && !rightFree)
            {
                transform.Rotate(0, -90, 0);
            }
            else if (rightFree && !leftFree)
            {
                transform.Rotate(0, 90, 0);
            }
            else if (leftFree && rightFree)
            {
                float randomTurn = Random.Range(0, 2) == 0 ? -90 : 90;
                transform.Rotate(0, randomTurn, 0);
            }
            else
            {
                transform.Rotate(0, 180, 0);
            }
        }
    }
    
    void MoveForward()
    {
        Vector3 movement = transform.forward * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }
    
    void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;
        
        Gizmos.color = Color.yellow;
        Vector3 rayOrigin = transform.position + Vector3.up * 0.5f;
        Gizmos.DrawRay(rayOrigin, transform.forward * rayDistance);
        Gizmos.DrawRay(rayOrigin, -transform.right * rayDistance);
        Gizmos.DrawRay(rayOrigin, transform.right * rayDistance);
    }
}