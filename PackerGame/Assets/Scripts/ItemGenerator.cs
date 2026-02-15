using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ItemSpawnData
{
    public GameObject itemPrefab;
    public Transform spawnPoint;
}

public class ItemGenerator : MonoBehaviour
{
    [Header("5 Boxes placed in the scene")]
    public BoxScript[] boxes = new BoxScript[5];

    [Header("Items that can be required")]
    public ItemType[] possibleItems;

    [Header("Items to spawn in the scene")]
    public List<ItemSpawnData> itemsToSpawn = new List<ItemSpawnData>();

    private void Start()
    {
        AssignItemsToBoxes();
        SpawnAllItems();   // ⭐ NEW
    }

    // ─────────────────────────────
    // Spawn all items once at start
    // ─────────────────────────────
    void SpawnAllItems()
    {
        if (itemsToSpawn == null || itemsToSpawn.Count == 0)
        {
            Debug.LogWarning("ItemGenerator: No items to spawn.");
            return;
        }

        foreach (ItemSpawnData data in itemsToSpawn)
        {
            if (data.itemPrefab == null || data.spawnPoint == null)
            {
                Debug.LogWarning("ItemGenerator: Missing prefab or spawn point.");
                continue;
            }

            Instantiate(data.itemPrefab, data.spawnPoint.position, data.spawnPoint.rotation);
        }
    }

    // ─────────────────────────────
    // Assign UNIQUE items to 5 boxes
    // ─────────────────────────────
    void AssignItemsToBoxes()
    {
        if (boxes == null || boxes.Length == 0)
        {
            Debug.LogError("ItemGenerator: No boxes assigned!");
            return;
        }

        if (possibleItems == null || possibleItems.Length < boxes.Length)
        {
            Debug.LogError("ItemGenerator: Not enough possible items for unique assignment!");
            return;
        }

        // Shuffle items
        ItemType[] shuffled = (ItemType[])possibleItems.Clone();

        for (int i = 0; i < shuffled.Length; i++)
        {
            int rand = Random.Range(i, shuffled.Length);
            (shuffled[i], shuffled[rand]) = (shuffled[rand], shuffled[i]);
        }

        // Assign first 5 unique items
        for (int i = 0; i < boxes.Length; i++)
        {
            if (boxes[i] != null)
                boxes[i].SetRequiredItem(shuffled[i]);
        }
    }
}