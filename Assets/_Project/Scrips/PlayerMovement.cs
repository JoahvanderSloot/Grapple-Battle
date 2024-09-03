using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public float wallJumpForce;
    bool readyToJump;

    [HideInInspector] public float walkSpeed;
    [HideInInspector] public float sprintSpeed;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    [SerializeField] private CapsuleCollider playerCollider;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
    }

    private void Update()
    {
        // ground check
        float playerHeight = playerCollider.height * playerCollider.gameObject.transform.localScale.y;
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.1f, whatIsGround);

        MyInput();
        SpeedControl();

        // handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        if (transform.position.y <= -10)
        {
            transform.position = new Vector3(0, 0, 0);
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
        WallJump();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on ground 
        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void WallJump()
    {
        if (grounded || !Input.GetKey(jumpKey) || !readyToJump) return;

        float playerHeight = playerCollider.height * playerCollider.gameObject.transform.localScale.y;
        float detectionRadius = playerHeight * 0.5f - 0.3f;
        Vector3 wallJumpSpherePos = new Vector3(transform.position.x, transform.position.y / 1.5f, transform.position.z);

        Collider[] hitColliders = Physics.OverlapSphere(wallJumpSpherePos, detectionRadius, whatIsGround);
        bool playerCanWalljump = false;
        Vector3 wallPosition = Vector3.zero;

        foreach (Collider hitCollider in hitColliders)
        {
            wallPosition = hitCollider.ClosestPoint(transform.position);
            playerCanWalljump = true;
            break;
        }

        if (playerCanWalljump)
        {
            readyToJump = false;

            // Calculate the direction away from the wall
            Vector3 wallDirection = (transform.position - wallPosition).normalized;
            Vector3 jumpDirection = Vector3.up + wallDirection;

            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(jumpDirection * wallJumpForce, ForceMode.Impulse);

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void OnDrawGizmos()
    {
        // Draw gizmo for the ground detection ray
        Vector3 endPosition = transform.position;
        float playerHeight = playerCollider.height * playerCollider.gameObject.transform.localScale.y;
        endPosition.y -= playerHeight * 0.5f + 0.3f;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, endPosition);

        float detectionRadius = playerHeight * 0.5f - 0.3f;
        Vector3 wallJumpSpherePos = new Vector3(transform.position.x, transform.position.y / 0.5f, transform.position.z);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(wallJumpSpherePos, detectionRadius);
    }
}
