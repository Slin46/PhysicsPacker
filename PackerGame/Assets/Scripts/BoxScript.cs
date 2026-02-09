using UnityEngine;

public class BoxScript : MonoBehaviour
{
    [Header("Box Settings")]
    public Transform boxInteriorPoint;   // where item snaps inside
    public GameObject triggerPoint;
    public bool isPacked = false;

    private RoundManager roundManager;

    private void Start()
    {
        roundManager = FindFirstObjectByType<RoundManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Only react to grabbable items
        if (!other.CompareTag("Grabbable")) return;
        if (isPacked) return;

        PackItem(other.gameObject);
    }

    void PackItem(GameObject itemObj)
{
    isPacked = true;

    // Stop physics
    Rigidbody rb = itemObj.GetComponent<Rigidbody>();
    if (rb != null)
    {
        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true;
    }

    // Detach from player
    FixedJoint joint = itemObj.GetComponent<FixedJoint>();
    if (joint != null)
        Destroy(joint);

    // Snap to box interior
    itemObj.transform.position = boxInteriorPoint.position;
    itemObj.transform.rotation = boxInteriorPoint.rotation;
    itemObj.transform.SetParent(boxInteriorPoint);

    Debug.Log("Item packed!");

    // Notify RoundManager with ItemType
    Item itemComponent = itemObj.GetComponent<Item>();
    if (itemComponent != null && roundManager != null)
    {
        roundManager.OnItemPacked(itemComponent.itemType);
    }
}
}
