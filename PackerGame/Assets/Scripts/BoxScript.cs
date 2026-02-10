using System.Collections;
using UnityEngine;

public class BoxScript : MonoBehaviour
{
    [Header("Box Settings")]
    private Rigidbody boxRb;
    public Transform boxInteriorPoint;   // where item snaps inside
    public GameObject triggerPoint;
    public bool isPacked = false;
    public float maxSpeed = 3f;

    private RoundManager roundManager;

    private void Start()
    {
        roundManager = RoundManager.Instance;
        boxRb = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        if (boxRb == null) return;

        if (boxRb.linearVelocity.magnitude > maxSpeed)
        {
            boxRb.linearVelocity = boxRb.linearVelocity.normalized * maxSpeed;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // Only react to collectible items
        if (!other.CompareTag("Collectible")) return;
        if (isPacked) return;

        PackItem(other.gameObject);

    }

    void PackItem(GameObject itemObj)
    {
        isPacked = true;

        StartCoroutine(PackNextPhysicsFrame(itemObj));
    }

    IEnumerator PackNextPhysicsFrame(GameObject itemObj)
    {
        Rigidbody rb = itemObj.GetComponent<Rigidbody>();
        Collider col = itemObj.GetComponent<Collider>();

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        if (col != null)
            col.enabled = false;

        yield return new WaitForFixedUpdate();

        itemObj.transform.SetParent(transform);
        itemObj.transform.position = boxInteriorPoint.position;
        itemObj.transform.rotation = boxInteriorPoint.rotation;

        Item itemComponent = itemObj.GetComponent<Item>();
        if (itemComponent != null && roundManager != null)
        {
            roundManager.OnItemPacked(itemComponent.itemType);
        }
    }

}
