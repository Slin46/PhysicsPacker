using UnityEngine;

public class BoxScript : MonoBehaviour
{
    [Header("Box Settings")]
    public Transform boxInteriorPoint;   // where item snaps inside
    public bool isPacked = false;

    private RoundManager roundManager;

    private void Start()
    {
        roundManager = FindObjectOfType<RoundManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Only react to grabbable items
        if (!other.CompareTag("Grabbable")) return;
        if (isPacked) return;

        PackItem(other.gameObject);
    }

    void PackItem(GameObject item)
    {
        isPacked = true;

        // --- Stop physics ---
        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        // --- Detach from player hand (destroy FixedJoint if exists) ---
        FixedJoint joint = item.GetComponent<FixedJoint>();
        if (joint != null)
            Destroy(joint);

        // --- Snap to box interior ---
        item.transform.position = boxInteriorPoint.position;
        item.transform.rotation = boxInteriorPoint.rotation;

        // --- Optional: parent to box so it stays ---
        item.transform.SetParent(boxInteriorPoint);

        Debug.Log("Item packed!");

        // --- Notify round manager ---
        if (roundManager != null)
            roundManager.OnItemPacked(item.name);
    }
}
