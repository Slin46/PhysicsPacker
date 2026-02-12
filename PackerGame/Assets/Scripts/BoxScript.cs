using System.Collections;
using TMPro;
using UnityEngine;

public class BoxScript : MonoBehaviour
{
    [Header("Box Settings")]
    private Rigidbody boxRb;
    public Transform boxInteriorPoint; // where item snaps inside
    public bool isPacked = false;
    public float maxSpeed = 3f;

    [Header("Order")]
    public ItemType requiredItem; // each box has its own required item

    [Header("Floating UI")]
    public TextMeshPro worldTextPrefab; // prefab for floating text
    private TextMeshPro worldTextInstance; // instantiated text
    public Vector3 textOffset = new Vector3(0, 2f, 0);

    private RoundManager roundManager;

    private void Start()
    {
        roundManager = FindFirstObjectByType<RoundManager>();
        boxRb = GetComponent<Rigidbody>();

        // spawn the floating text above the box
        if (worldTextPrefab != null)
        {
            float yOffset = 0f;
            Collider col = GetComponent<Collider>();
            if (col != null) yOffset = col.bounds.size.y + 0.1f; // just above box
            worldTextInstance = Instantiate(worldTextPrefab, transform.position + new Vector3(0, yOffset, 0), Quaternion.Euler(90f, 0f, 0f));
            worldTextInstance.text = requiredItem.ToString();
        }
    }

    private void LateUpdate()
    {
        // make the floating text follow the box
        if (worldTextInstance != null)
            worldTextInstance.transform.position = transform.position + textOffset;
    }

    private void FixedUpdate()
    {
        if (boxRb == null) return;

        if (boxRb.linearVelocity.magnitude > maxSpeed)
            boxRb.linearVelocity = boxRb.linearVelocity.normalized * maxSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Collectible") || isPacked) return;

        Item item = other.GetComponent<Item>();
        if (item == null) return;

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

        // hide floating text
        if (worldTextInstance != null)
            worldTextInstance.gameObject.SetActive(false);

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

        // notify RoundManager
        if (roundManager != null)
            roundManager.OnBoxCompleted(this);
    }

    // called by ItemGenerator to assign the required item
    public void SetRequiredItem(ItemType item)
    {
        requiredItem = item;

        // Update floating text
        if (worldTextInstance != null)
            worldTextInstance.text = requiredItem.ToString();
    }
}