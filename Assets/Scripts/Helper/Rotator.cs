using UnityEngine;

namespace Helper
{
    public class Rotator : MonoBehaviour
    {
        [SerializeField] private Vector3 _rotationSpeed = new Vector3(0, 50, 0);

        private void Update()
        {
            transform.Rotate(_rotationSpeed * Time.deltaTime);
        }
    }
}