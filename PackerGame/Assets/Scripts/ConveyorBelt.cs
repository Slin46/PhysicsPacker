using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public Vector3 moveDirection = Vector3.right;
    public float speed = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb == null) return;

        // Move object along belt
        rb.linearVelocity = new Vector3(
            moveDirection.normalized.x * speed,
            rb.linearVelocity.y,
            moveDirection.normalized.z * speed
        );
    }
}
