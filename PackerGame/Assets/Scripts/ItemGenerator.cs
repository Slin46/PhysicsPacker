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
    [Header("Boxes already in scene")]
    public BoxScript[] boxes;

    [Header("Possible required items (>= box count)")]
    public ItemType[] possibleItems;

    [Header("Collectible items to spawn")]
    public List<ItemSpawnData> itemsToSpawn = new List<ItemSpawnData>();

    void Start()
    {
        AssignItemsToBoxes();
        SpawnAllItems();
    }

    // ─────────────────────────────
    // Assign UNIQUE items + spawn correct text
    // ─────────────────────────────
    void AssignItemsToBoxes()
    {
        if (boxes.Length == 0)
        {
            Debug.LogError("ItemGenerator: No boxes assigned.");
            return;
        }

        if (possibleItems.Length < boxes.Length)
        {
            Debug.LogError("ItemGenerator: Not enough possible items.");
            return;
        }

        // Shuffle items
        ItemType[] shuffled = (ItemType[])possibleItems.Clone();

        for (int i = 0; i < shuffled.Length; i++)
        {
            int rand = Random.Range(i, shuffled.Length);
            (shuffled[i], shuffled[rand]) = (shuffled[rand], shuffled[i]);
        }

        // Initialize each box with its item
        for (int i = 0; i < boxes.Length; i++)
        {
            boxes[i].Initialize(shuffled[i]);
        }
    }

    // ─────────────────────────────
    // Spawn collectible items
    // ─────────────────────────────
    void SpawnAllItems()
    {
        foreach (var data in itemsToSpawn)
        {
            if (data.itemPrefab == null || data.spawnPoint == null) continue;

            Instantiate(data.itemPrefab, data.spawnPoint.position, data.spawnPoint.rotation);
        }
    }
}
