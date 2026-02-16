using UnityEngine;

public class GrabScript : MonoBehaviour
{
    //radius so it can grab stuff that is out of reach
    public float grabRadius = 0.3f;
    public LayerMask grabbableLayer;
    //how hard/far it will be thrown
    public float throwForwardForce = 2f;
    public float throwUpForce = 1.5f;

    private Rigidbody grabbedRb;
    private FixedJoint joint;

    void Update()
    {
        if (Input.GetMouseButton(0)) // holding grab
        {
            if (grabbedRb == null)
            {
                TryGrab();
            }
        }
        else // release
        {
            Release();
        }

        // throw: right mouse button
        if (Input.GetKeyDown(KeyCode.F) && grabbedRb != null)
        {
            Throw();
        }
    }

    void TryGrab()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, grabRadius, grabbableLayer);
        if (hits.Length == 0) return;

        // closest object
        Collider closest = null;
        float minDist = Mathf.Infinity;

        foreach (Collider hit in hits)
        {
            float dist = Vector3.Distance(transform.position, hit.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = hit;
            }
        }

        grabbedRb = closest.GetComponentInParent<Rigidbody>();
        if (grabbedRb == null) return;

        //connects object to joint
        joint = gameObject.AddComponent<FixedJoint>();
        joint.connectedBody = grabbedRb;
        joint.breakForce = Mathf.Infinity;
        joint.breakTorque = Mathf.Infinity;
    }

    void Release()
    {
        //destroy join to destroy connection
        if (joint != null)
        {
            Destroy(joint);
            joint = null;
            grabbedRb = null;
        }
    }

    void Throw()
    {
        if (grabbedRb == null) return;

        // remove joint first
        Destroy(joint);
        joint = null;

        // calculate throw direction: hand forward + up
        Vector3 throwDir = transform.forward * throwForwardForce + Vector3.up * throwUpForce;

        // apply velocity instantly
        grabbedRb.linearVelocity = throwDir;

        grabbedRb = null; // hand is now free
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, grabRadius);
    }
    public void ForceRelease()
    {
        if (joint != null)
        {
            Destroy(joint);
            joint = null;
        }

        grabbedRb = null;
    }
}
