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

    private void Start()
    {
        SpawnAllItems();
        SpawnInitialBoxes();
    }

    /// <summary>
    /// Spawn all collectible items at their spawn points
    /// </summary>
    public void SpawnAllItems()
    {
        if (items.Count == 0) return;

        foreach (ItemSpawnData data in items)
        {
            if (data.itemPrefab != null && data.spawnPoint != null)
                Instantiate(data.itemPrefab, data.spawnPoint.position, data.spawnPoint.rotation);
        }
    }

    /// <summary>
    /// Spawn the first set of boxes with unique required items
    /// </summary>
    void SpawnInitialBoxes()
    {
        if (boxPrefab == null || boxSpawnPoints.Count == 0 || possibleItems.Length == 0) return;

        // Shuffle possible items so each box gets a different one
        ItemType[] shuffledItems = (ItemType[])possibleItems.Clone();
        ShuffleArray(shuffledItems);

        for (int i = 0; i < startBoxCount; i++)
        {
            Transform spawnPoint = boxSpawnPoints[i % boxSpawnPoints.Count];
            GameObject boxObj = Instantiate(boxPrefab, spawnPoint.position, spawnPoint.rotation);

            BoxScript box = boxObj.GetComponent<BoxScript>();
            if (box != null)
            {
                // Assign unique required item
                box.SetRequiredItem(shuffledItems[i % shuffledItems.Length]);
            }
        }
    }

    /// <summary>
    /// Spawn a replacement box after one is packed
    /// </summary>
    public void SpawnReplacementBox()
    {
        if (boxPrefab == null || boxSpawnPoints.Count == 0 || possibleItems.Length == 0) return;

        Transform point = boxSpawnPoints[Random.Range(0, boxSpawnPoints.Count)];
        GameObject boxObj = Instantiate(boxPrefab, point.position, point.rotation);

        BoxScript box = boxObj.GetComponent<BoxScript>();
        if (box != null)
        {
            box.SetRequiredItem(possibleItems[Random.Range(0, possibleItems.Length)]);
        }
    }

    /// <summary>
    /// Shuffle array helper
    /// </summary>
    void ShuffleArray(ItemType[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            ItemType temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }
}
