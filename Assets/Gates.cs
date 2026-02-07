using UnityEngine;

public class Gates : MonoBehaviour
{
    private int score =0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Destroy(other.gameObject);
            score++;

            Debug.Log($"Score: {score}");
        }
    }
}
