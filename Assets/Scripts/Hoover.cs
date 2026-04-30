using UnityEngine;

public class Hoover : MonoBehaviour
{
    public float speed = 5f;
    public float rayDistance = 2f;
    public LayerMask obstacleLayer;
    
    private Rigidbody rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }
    
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        Vector3 movement = new Vector3(horizontal, 0, vertical);
        if (movement.magnitude > 1f)
            movement.Normalize();
        
        rb.velocity = movement * speed;
        
        DetectObstacles();
    }
    
    void DetectObstacles()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, rayDistance, obstacleLayer))
        {
            Debug.DrawRay(transform.position, transform.forward * rayDistance, Color.red);
            Debug.Log($"Впереди препятствие: {hit.collider.name}");
        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * rayDistance, Color.green);
        }
        
        if (Physics.Raycast(transform.position, -transform.right, out hit, rayDistance, obstacleLayer))
        {
            Debug.DrawRay(transform.position, -transform.right * rayDistance, Color.red);
            Debug.Log($"Слева препятствие: {hit.collider.name}");
        }
        else
        {
            Debug.DrawRay(transform.position, -transform.right * rayDistance, Color.green);
        }
        
        if (Physics.Raycast(transform.position, transform.right, out hit, rayDistance, obstacleLayer))
        {
            Debug.DrawRay(transform.position, transform.right * rayDistance, Color.red);
            Debug.Log($"Справа препятствие: {hit.collider.name}");
        }
        else
        {
            Debug.DrawRay(transform.position, transform.right * rayDistance, Color.green);
        }
    }
}
