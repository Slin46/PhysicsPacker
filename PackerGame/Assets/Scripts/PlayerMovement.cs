using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //movement, sprinting, jumping force
    public float moveForce = 60f;
    public float maxSpeed = 5f;
    public float jumpForce = 7f;

    //groundcheck
    public Transform groundCheck;
    public float groundRadius = 0.3f;
    public LayerMask groundMask;

    //grabpoint assigned
    public Transform grabPoint;
    public float grabRange = 2f;

    //assign 3d rigidbody
    private Rigidbody rb;
    //check if the player is grounded
    private bool isGrounded;

    private Rigidbody grabbedObject;
    private ConfigurableJoint grabJoint;

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
        // Ground check (visualize this!)
        isGrounded = Physics.CheckSphere(
            groundCheck.position,
            groundRadius,
            groundMask
        );

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        HandleGrab();
        HandleDrop();


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

        // Rotate COM to face movement direction
        if (moveDir.magnitude > 0.1f)
        {
            com.transform.rotation = Quaternion.Slerp(
                com.transform.rotation,
                Quaternion.LookRotation(moveDir),
                Time.fixedDeltaTime * 10f
            );
        }
    }


    void HandleGrab()
    {
        //left click object with grab it
        if (Input.GetMouseButtonDown(0) && grabbedObject == null)
        {
            //detects the cursor and checks if the object is within range
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, grabRange))
            {
                //if the object has the tag grabbable it will move to the grab point
                if (hit.collider.CompareTag("Grabbable"))
                {
                    grabbedObject = hit.collider.GetComponent<Rigidbody>();
                    GrabObject(grabbedObject);
                }
            }
        }
    }
    void GrabObject(Rigidbody obj)
    {
        // Only create joint on the grabPoint (arm)
        grabJoint = grabPoint.gameObject.AddComponent<ConfigurableJoint>();
        grabJoint.connectedBody = obj;

        // Lock position so it stays attached
        grabJoint.xMotion = ConfigurableJointMotion.Locked;
        grabJoint.yMotion = ConfigurableJointMotion.Locked;
        grabJoint.zMotion = ConfigurableJointMotion.Locked;

        // Allow rotation for floppy physics
        grabJoint.angularXMotion = ConfigurableJointMotion.Free;
        grabJoint.angularYMotion = ConfigurableJointMotion.Free;
        grabJoint.angularZMotion = ConfigurableJointMotion.Free;

        // Anchor at hand
        grabJoint.anchor = Vector3.zero;
        grabJoint.autoConfigureConnectedAnchor = false;
        grabJoint.connectedAnchor = Vector3.zero;

        // Makes object feel heavy
        obj.linearDamping = 10f;
    }
    void HandleDrop()
    {
        //if the player has an object and presses F they can drop the item
        if (Input.GetKeyDown(KeyCode.F) && grabbedObject != null)
        {
            //destroy grab point so item can be dropped
            Destroy(grabJoint);
            grabbedObject.linearDamping = 0f;
            grabbedObject = null;
        }
    }
}
