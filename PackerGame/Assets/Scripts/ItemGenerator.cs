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
    public ItemType[] possibleItems;

    private void Start()
    {
        SpawnAllItems();
        SpawnStartingBoxes();
    }

    // ─────────────────────────────
    // Spawn all items once
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
    // Spawn first 5 boxes
    // ─────────────────────────────
    void SpawnStartingBoxes()
    {
        for (int i = 0; i < startBoxCount && i < boxSpawnPoints.Count; i++)
        {
            SpawnBoxAt(boxSpawnPoints[i]);
        }
    }

    // ─────────────────────────────
    // Spawn ONE replacement box
    // Called when correct item packed
    // ─────────────────────────────
public void SpawnReplacementBox()
{
    if (boxPrefab == null || boxSpawnPoints.Count == 0) return;

    Transform point = boxSpawnPoints[Random.Range(0, boxSpawnPoints.Count)];

    GameObject boxObj = Instantiate(boxPrefab, point.position, point.rotation);

    // ⭐ Assign random required item to this box
    BoxScript box = boxObj.GetComponent<BoxScript>();
    if (box != null)
        box.requiredItem = possibleItems[Random.Range(0, possibleItems.Length)];
}
    void SpawnBoxAt(Transform point)
    {
        Instantiate(boxPrefab, point.position, point.rotation);
    }
}
