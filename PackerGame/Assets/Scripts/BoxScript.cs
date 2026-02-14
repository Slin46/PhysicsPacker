using System.Collections;
using TMPro;
using UnityEngine;

public class BoxScript : MonoBehaviour
{
    [Header("Required Item")]
    public ItemType requiredItem;

    [Header("Pack Point")]
    public Transform boxInteriorPoint;

    [Header("Floating Text")]
    public TextMeshPro worldTextPrefab;
    private TextMeshPro worldTextInstance;
    public Vector3 textOffset = new Vector3(0, 2f, 0);

   public bool isPacked = false;
    private RoundManager roundManager;

    void Start()
    {
        roundManager = FindFirstObjectByType<RoundManager>();

        // Spawn floating text
        if (worldTextPrefab != null)
        {
            worldTextInstance = Instantiate(worldTextPrefab, transform.position + textOffset, Quaternion.identity);
            worldTextInstance.text = requiredItem.ToString();
        }
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

        if (item.itemType != requiredItem)
        {
            Debug.Log("Wrong box!");
            return;
        }

        StartCoroutine(PackItem(other.gameObject));
    }

    IEnumerator PackItem(GameObject itemObj)
    {
        isPacked = true;

        if (worldTextInstance != null)
            worldTextInstance.gameObject.SetActive(false);

        Rigidbody rb = itemObj.GetComponent<Rigidbody>();
        Collider col = itemObj.GetComponent<Collider>();

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
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

    // Called by ItemGenerator when spawning
    public void SetRequiredItem(ItemType item)
    {
        requiredItem = item;

        if (worldTextInstance != null)
            worldTextInstance.text = requiredItem.ToString();
    }
}