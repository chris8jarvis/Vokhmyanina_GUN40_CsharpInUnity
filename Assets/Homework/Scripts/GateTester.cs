using UnityEngine;

public class GateTester : MonoBehaviour
{
    public GameObject ballPrefab;
    public Vector3 ballSpawnPosition = new Vector3(0, 1, 0);
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            TestGates();
        }
    }
    
    void TestGates()
    {
        // Создаём мяч в заданной позиции
        GameObject ball = Instantiate(ballPrefab, ballSpawnPosition, Quaternion.identity);
        
        // Даём толчок вперёд
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(Vector3.forward * 5, ForceMode.Impulse);
        }
        
        Debug.Log("Test ball created at: " + ball.transform.position);
    }
}