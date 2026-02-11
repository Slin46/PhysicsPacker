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

    void SpawnAllItems()
    {
        foreach (var data in items)
        {
            if (data.itemPrefab == null || data.spawnPoint == null)
                continue;

            Instantiate(data.itemPrefab, data.spawnPoint.position, data.spawnPoint.rotation);
        }
    }

    void SpawnInitialBoxes()
    {
        if (possibleItems.Length < startBoxCount)
        {
            Debug.LogWarning("Not enough unique items to assign to each box!");
        }

        // Create a temporary list of items so we can remove assigned ones
        List<ItemType> remainingItems = new List<ItemType>(possibleItems);

        for (int i = 0; i < startBoxCount; i++)
        {
            Transform point = boxSpawnPoints[i % boxSpawnPoints.Count];
            SpawnBox(point, remainingItems);
        }
    }

    // Spawn one box with a unique required item
    void SpawnBox(Transform point, List<ItemType> remainingItems)
    {
        GameObject boxObj = Instantiate(boxPrefab, point.position, point.rotation);
        BoxScript box = boxObj.GetComponent<BoxScript>();
        if (box == null) return;

        // Pick a random item from remaining list
        int index = Random.Range(0, remainingItems.Count);
        ItemType item = remainingItems[index];
        remainingItems.RemoveAt(index); // remove to prevent duplicates

        box.SetRequiredItem(item);
        box.SetGenerator(this); // allow box to request replacement box
    }

    // Spawn a replacement box after one is completed
    public void SpawnReplacementBox()
    {
        if (boxPrefab == null || boxSpawnPoints.Count == 0 || possibleItems.Length == 0) return;

        Transform point = boxSpawnPoints[Random.Range(0, boxSpawnPoints.Count)];
        GameObject boxObj = Instantiate(boxPrefab, point.position, point.rotation);

        BoxScript box = boxObj.GetComponent<BoxScript>();
        if (box == null) return;

        // Replacement box can pick ANY item randomly
        box.SetRequiredItem(possibleItems[Random.Range(0, possibleItems.Length)]);
        box.SetGenerator(this);
    }
}