using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 8f;
    [SerializeField] private float _rotationSpeed = 720f;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _groundCheckDistance = 0.2f;

    [Header("Ground Check")]
    [SerializeField] private LayerMask _groundLayer;

    private Rigidbody _rb;
    private Vector3 _moveDirection;
    private bool _isGrounded;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal"); 
        float vertical = Input.GetAxis("Vertical"); 

        
        _moveDirection = new Vector3(horizontal, 0, vertical).normalized;

        
        _isGrounded = Physics.CheckSphere(transform.position - Vector3.up * 0.1f, _groundCheckDistance, _groundLayer);

        
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
        RotatePlayer();
    }

    private void MovePlayer()
    {
        if (_moveDirection.magnitude < 0.01f) return;

        Vector3 targetPosition = transform.position + _moveDirection * _moveSpeed * Time.fixedDeltaTime;
        _rb.MovePosition(targetPosition);
    }

    private void RotatePlayer()
    {
        if (_moveDirection.magnitude < 0.01f) return;

        Quaternion targetRotation = Quaternion.LookRotation(_moveDirection);
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            _rotationSpeed * Time.fixedDeltaTime
        );
    }

    private void Jump()
    {
        _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        _isGrounded = false; 
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position - Vector3.up * 0.1f, _groundCheckDistance);
    }
}