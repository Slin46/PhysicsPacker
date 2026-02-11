using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //movement, jumping force
    public float moveForce = 60f;
    public float maxSpeed = 5f;
    public float jumpForce = 100f;
    public float rotationSpeed = 5f;

    //groundcheck
    public Transform groundCheck;
    public float groundRadius = 0.3f;
    public LayerMask groundMask;

    //assign 3d rigidbody
    private Rigidbody rb;
    //check if the player is grounded
    private bool isGrounded;

    public GameObject com;
    public Transform cam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if player is on the ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, groundMask);

        // Jump when space is pressed and player is grounded
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

    }

    void Jump()
    {
        Debug.Log(rb.angularVelocity.y);
        
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        // Apply upward force
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    

    void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 camForward = cam.forward;
        camForward.y = 0f;
        camForward.Normalize();

        Vector3 camRight = cam.right;
        camRight.y = 0f;
        camRight.Normalize();

        Vector3 moveDir = (camForward * z + camRight * x).normalized;

        // Apply force (use Acceleration for ragdoll)
        rb.AddForce(moveDir * moveForce, ForceMode.Acceleration);

        // Clamp horizontal speed
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if (flatVel.magnitude > maxSpeed)
        {
            Vector3 limited = flatVel.normalized * maxSpeed;
            rb.linearVelocity = new Vector3(limited.x, rb.linearVelocity.y, limited.z);
        }

        //rotate towards move direction
        if(moveDir!= Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            rb.MoveRotation(targetRotation);
        }

    }

}
