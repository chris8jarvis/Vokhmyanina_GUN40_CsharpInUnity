using UnityEngine;

public class ThrowBall : MonoBehaviour
{
    public GameObject ball;           
    public Transform spawnPoint;      
    public float forceMultiplier = 15f;
    public float maxDragDistance = 5f;
    public BowlingScore scoreSystem;
    
    private Vector3 dragStartPos;
    private bool isDragging = false;
    private GameObject currentBall;
    private LineRenderer lineRenderer;
    private bool ballMoving = false;
    private bool waitingForReset = false;

    void Start()
    {
        SpawnNewBall();
        
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }

    void Update()
    {
        if (currentBall == null) return;
        if (waitingForReset) return;
        
        if (ballMoving)
        {
            Rigidbody rb = currentBall.GetComponent<Rigidbody>();
            if (rb.velocity.magnitude < 0.1f)
            {
                ballMoving = false;
                waitingForReset = true;
                
                if (scoreSystem != null)
                {
                    scoreSystem.EndThrow();
                }
                
                Invoke("DestroyAndSpawn", 2f);
            }
        }
        
        if (!ballMoving && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == currentBall)
            {
                isDragging = true;
                dragStartPos = Input.mousePosition;
                lineRenderer.enabled = true;
            }
        }
        
        if (isDragging && Input.GetMouseButton(0))
        {
            Vector3 currentPos = Input.mousePosition;
            Vector3 dragDelta = dragStartPos - currentPos;
            dragDelta.z = dragDelta.y;
            Vector3 direction = new Vector3(dragDelta.x, 0, dragDelta.y).normalized;
            
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, currentBall.transform.position);
            lineRenderer.SetPosition(1, currentBall.transform.position + direction * 3f);
        }
        
        if (isDragging && Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            lineRenderer.enabled = false;
            
            Vector3 dragEndPos = Input.mousePosition;
            Vector3 dragVector = dragStartPos - dragEndPos;
            float force = Mathf.Clamp(dragVector.magnitude / 10f, 2f, maxDragDistance);
            
            Vector3 forceDirection = new Vector3(dragVector.x, 0, dragVector.y).normalized;
            
            Rigidbody rb = currentBall.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.AddForce(forceDirection * force * forceMultiplier, ForceMode.Impulse);
            
            ballMoving = true;
        }
    }

    void DestroyAndSpawn()
    {
        waitingForReset = false;
        SpawnNewBall();
    }

    void SpawnNewBall()
    {
        if (currentBall != null)
            Destroy(currentBall);
        
        currentBall = Instantiate(ball, spawnPoint.position, Quaternion.identity);
        Rigidbody rb = currentBall.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        ballMoving = false;
    }
}