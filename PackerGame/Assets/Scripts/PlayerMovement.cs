using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //movement, sprinting, jumping force
    public float moveForce = 30f;
    public float sprintMultiplier = 1.5f;
    public float jumpForce = 7f;

    //groundcheck
    public Transform groundCheck;
    public float groundDistance = 0.3f;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleJump();
        HandleGrab();
        HandleDrop();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMovement()
    {

        float x = Input.GetAxis("Horizontal"); // A/D
        float z = Input.GetAxis("Vertical");   // W/S

        // Move relative to player orientation
        Vector3 moveDir = (transform.forward * z + transform.right * x).normalized;

        float currentForce = moveForce;
        if (Input.GetKey(KeyCode.LeftShift))
            currentForce *= sprintMultiplier;

        rb.AddForce(moveDir * currentForce, ForceMode.Force);

        // Clamp horizontal velocity
        Vector3 horizontalVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if (horizontalVel.magnitude > 5f) // max speed
        {
            Vector3 limitedVel = horizontalVel.normalized * 5f;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }
    void HandleJump()
    {
        //check if the player is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //space bar to jump if player is grounded
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
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
