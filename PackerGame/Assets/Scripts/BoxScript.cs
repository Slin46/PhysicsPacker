using System.Collections;
using UnityEngine;
using TMPro;

public class BoxScript : MonoBehaviour
{
    [Header("Box Settings")]
    private Rigidbody boxRb;

    public Transform boxInteriorPoint;
    public bool isPacked = false;
    public float maxSpeed = 3f;

    [Header("Order")]
    public ItemType requiredItem;

    [Header("Floating UI")]
    public TextMeshPro worldText;   // ⭐ floating text above box
    public Vector3 textOffset = new Vector3(0, 2f, 0);

    private RoundManager roundManager;
    private ItemGenerator generator;
    private void Start()
    {
        roundManager = FindFirstObjectByType<RoundManager>();
        boxRb = GetComponent<Rigidbody>();

        UpdateFloatingText();
    }

    void FixedUpdate()
    {
        if (boxRb == null) return;

        if (boxRb.linearVelocity.magnitude > maxSpeed)
            boxRb.linearVelocity = boxRb.linearVelocity.normalized * maxSpeed;
    }

    private void LateUpdate()
    {
        // keep text floating above box
        if (worldText != null)
            worldText.transform.position = transform.position + textOffset;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Collectible")) return;
        if (isPacked) return;

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

        // hide floating text when completed
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

        if (roundManager != null)
            roundManager.OnBoxCompleted(this);
    }

    void UpdateFloatingText()
    {
        if (worldText != null)
            worldText.text = requiredItem.ToString();
    }

    //called when generator assigns item
    public void SetRequiredItem(ItemType item)
    {
        requiredItem = item;
        UpdateFloatingText();
    }
    public void SetGenerator(ItemGenerator gen)
    {
    generator = gen;
    }
}