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
    [Header("Items to spawn at round start")]
    public List<ItemSpawnData> items = new List<ItemSpawnData>();

    [Header("Box spawning")]
    public GameObject boxPrefab;
    public List<Transform> boxSpawnPoints = new List<Transform>();
    public int startBoxCount = 5;

    [Header("Possible required items for boxes")]
    public ItemType[] possibleItems;

    // ─────────────────────────────
    // Start
    // ─────────────────────────────
    private void Start()
    {
        SpawnAllItems();
        SpawnInitialBoxes();
    }

    // ─────────────────────────────
    // Spawn all collectible items once
    // ─────────────────────────────
    public void SpawnAllItems()
    {
        if (items.Count == 0)
        {
            Debug.LogWarning("ItemGenerator: No items assigned.");
            return;
        }

        foreach (ItemSpawnData data in items)
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
    // Spawn the first set of boxes
    // ─────────────────────────────
    void SpawnInitialBoxes()
    {
        if (boxPrefab == null || boxSpawnPoints.Count == 0)
        {
            Debug.LogWarning("ItemGenerator: Box prefab or spawn points missing.");
            return;
        }

        for (int i = 0; i < startBoxCount; i++)
        {
            Transform point = boxSpawnPoints[i % boxSpawnPoints.Count];
            SpawnBoxAt(point);
        }
    }

    // ─────────────────────────────
    // Spawn ONE replacement box
    // Called by RoundManager when a box is completed
    // ─────────────────────────────
    public void SpawnReplacementBox()
    {
        if (boxPrefab == null || boxSpawnPoints.Count == 0) return;

        Transform point = boxSpawnPoints[Random.Range(0, boxSpawnPoints.Count)];
        SpawnBoxAt(point);
    }

    // ─────────────────────────────
    // Core box spawn logic
    // ─────────────────────────────
    void SpawnBoxAt(Transform point)
    {
        GameObject boxObj = Instantiate(boxPrefab, point.position, point.rotation);

        BoxScript box = boxObj.GetComponent<BoxScript>();
        if (box != null)
        {
            box.SetRequiredItem(GetRandomValidItem());
        }
        else
        {
            Debug.LogWarning("Spawned box is missing BoxScript.");
        }
    }

    // ─────────────────────────────
    // Get random item that is NOT None
    // ─────────────────────────────
    ItemType GetRandomValidItem()
    {
        if (possibleItems == null || possibleItems.Length == 0)
            return ItemType.None;

        ItemType item;

        do
        {
            item = possibleItems[Random.Range(0, possibleItems.Length)];
        }
        while (item == ItemType.None);

        return item;
    }
}