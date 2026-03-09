using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float jumpForce = 5f;

    [Header("Ground Detection")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

    private Rigidbody rb;
    private float currentSpeed;
    private Vector3 moveDirection;
    private bool isGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentSpeed = walkSpeed;
    }

    private void Update()
    {
        CheckGrounded();
        ProcessInputs();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void CheckGrounded()
    {
        if (groundCheck != null)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        }
    }

    private void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        moveDirection = (transform.right * moveX + transform.forward * moveZ).normalized;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = sprintSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void MovePlayer()
    {
        Vector3 targetVelocity = new Vector3(moveDirection.x * currentSpeed, rb.linearVelocity.y, moveDirection.z * currentSpeed);
        rb.linearVelocity = targetVelocity;
    }
}