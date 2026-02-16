using TMPro;
using UnityEngine;
using System.Collections;

public class BoxScript : MonoBehaviour
{
    [Header("Assigned at runtime")]
    public ItemType requiredItem;

    [Header("Pack Point")]
    public Transform boxInteriorPoint;

    [Header("Floating Text")]
    public TextMeshPro worldTextPrefab;
    private TextMeshPro worldTextInstance;
    public Vector3 textOffset = new Vector3(0, 2f, 0);

    public bool isPacked = false;
    private RoundManager roundManager;

    void Awake()
    {
        roundManager = FindFirstObjectByType<RoundManager>();
    }

    // 🔹 Called ONLY by ItemGenerator
    public void Initialize(ItemType item)
    {
        requiredItem = item;

        SpawnFloatingText();
        UpdateFloatingText();
    }

    void SpawnFloatingText()
    {
        if (worldTextPrefab == null) return;

        worldTextInstance = Instantiate(
            worldTextPrefab,
            transform.position + textOffset,
            Quaternion.identity
        );
    }

    void UpdateFloatingText()
    {
        if (worldTextInstance != null)
            worldTextInstance.text = requiredItem.ToString();
    }

    void LateUpdate()
    {
        if (worldTextInstance != null)
            worldTextInstance.transform.position = transform.position + textOffset;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isPacked) return;
        if (!other.CompareTag("Collectible")) return;

        Item item = other.GetComponent<Item>();
        if (item == null) return;

        if (item.itemType != requiredItem) return;

        StartCoroutine(PackItem(other.gameObject));
    }

    IEnumerator PackItem(GameObject itemObj)
    {
        isPacked = true;

        // hide floating text
        if (worldTextInstance != null)
            worldTextInstance.gameObject.SetActive(false);

        // ─────────────────────────
        // 1. Force player release
        // ─────────────────────────
        GrabScript grabber = FindFirstObjectByType<GrabScript>();
        if (grabber != null)
            grabber.ForceRelease();

        yield return null; // wait 1 frame so physics settles

        // ─────────────────────────
        // 2. Disable physics safely
        // ─────────────────────────
        Rigidbody rb = itemObj.GetComponent<Rigidbody>();
        Collider col = itemObj.GetComponent<Collider>();

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
            rb.detectCollisions = false;
        }

        if (col != null)
            col.enabled = false;

        // ─────────────────────────
        // 3. Parent inside box
        // ─────────────────────────
        itemObj.transform.SetParent(boxInteriorPoint);
        itemObj.transform.localPosition = Vector3.zero;
        itemObj.transform.localRotation = Quaternion.identity;

        // ─────────────────────────
        // 4. Notify round manager
        // ─────────────────────────
        if (roundManager != null)
            roundManager.OnBoxCompleted(this);
        // enable box grabbing after packing
        Rigidbody boxRb = GetComponent<Rigidbody>();
        if (boxRb != null)
        {
            boxRb.isKinematic = false;
        }

        // 🔹 Put box on grabbable layer
        gameObject.layer = LayerMask.NameToLayer("Grabbable");
    }

}
