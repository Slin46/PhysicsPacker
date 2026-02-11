using System.Collections;
using UnityEngine;
using TMPro;

public class BoxScript : MonoBehaviour
{
    [Header("Box Settings")]
    private Rigidbody boxRb;
    public Transform boxInteriorPoint;   // where the item snaps inside
    public bool isPacked = false;
    public float maxSpeed = 3f;

    [Header("Order")]
    public ItemType requiredItem;       // each box has its own required item

    [Header("Floating UI")]
    public TextMeshPro worldText;       // floating text above the box
    public Vector3 textOffset = new Vector3(0, 2f, 0);

    private void Start()
    {
        boxRb = GetComponent<Rigidbody>();
        UpdateFloatingText();
    }

    void FixedUpdate()
    {
        if (boxRb == null) return;

        // Limit box movement speed
        if (boxRb.linearVelocity.magnitude > maxSpeed)
            boxRb.linearVelocity = boxRb.linearVelocity.normalized * maxSpeed;
    }

    private void LateUpdate()
    {
        // Keep floating text above the box
        if (worldText != null)
            worldText.transform.position = transform.position + textOffset;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Collectible")) return;
        if (isPacked) return;

        Item item = other.GetComponent<Item>();
        if (item == null) return;

        // Check if item matches box requirement
        if (item.itemType != requiredItem)
        {
            Debug.Log("Wrong item for this box!");
            return;
        }

        PackItem(other.gameObject);
    }

    void PackItem(GameObject itemObj)
    {
        isPacked = true;

        // Hide floating text
        if (worldText != null)
            worldText.gameObject.SetActive(false);

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

        // Optional: notify round manager if you need
        // RoundManager.Instance?.OnBoxCompleted(this);
    }

    public void SetRequiredItem(ItemType item)
    {
    requiredItem = item;

    if (worldText != null)
        worldText.text = requiredItem.ToString(); // update floating text immediately
    }

    private void UpdateFloatingText()
    {
        if (worldText != null)
            worldText.text = requiredItem.ToString();
    }
}