using UnityEngine;
using TMPro;

public class BoxScript : MonoBehaviour
{
    [Header("Box Settings")]
    public Transform boxInteriorPoint;
    public bool isPacked = false;

    [Header("Order")]
    public ItemType requiredItem;

    [Header("Floating UI")]
    public TextMeshPro worldTextPrefab;   // assign prefab here
    private TextMeshPro worldText;        // instance for this box
    public Vector3 textOffset = new Vector3(0, 2f, 0);

    private void Start()
    {
        // Instantiate the floating text for this box
        if (worldTextPrefab != null)
        {
            worldText = Instantiate(worldTextPrefab, transform.position + textOffset, Quaternion.identity);
            worldText.transform.SetParent(transform); // so it moves with box
            UpdateFloatingText();
        }
    }

    private void LateUpdate()
    {
        if (worldText != null)
            worldText.transform.position = transform.position + textOffset;
    }

    public void SetRequiredItem(ItemType item)
    {
        requiredItem = item;
        UpdateFloatingText();
    }

    void UpdateFloatingText()
    {
        if (worldText != null)
            worldText.text = requiredItem.ToString();
    }
}