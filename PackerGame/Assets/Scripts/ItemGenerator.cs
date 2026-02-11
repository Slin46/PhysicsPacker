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

    // Spawn all collectible items
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

    // Spawn initial boxes with unique required items
    void SpawnInitialBoxes()
    {
        if (boxPrefab == null || boxSpawnPoints.Count == 0)
        {
            Debug.LogWarning("ItemGenerator: Box prefab or spawn points missing.");
            return;
        }

        List<ItemType> itemsCopy = new List<ItemType>(possibleItems);

        for (int i = 0; i < startBoxCount; i++)
        {
            Transform point = boxSpawnPoints[i % boxSpawnPoints.Count];
            SpawnBoxAt(point, itemsCopy);
        }
    }

    // Spawn ONE replacement box after a box is packed
    public void SpawnReplacementBox()
    {
        if (boxPrefab == null || boxSpawnPoints.Count == 0) return;

        Transform point = boxSpawnPoints[Random.Range(0, boxSpawnPoints.Count)];
        List<ItemType> itemsCopy = new List<ItemType>(possibleItems);
        SpawnBoxAt(point, itemsCopy);
    }

    // Core box spawn logic
    void SpawnBoxAt(Transform point, List<ItemType> availableItems)
    {
        GameObject boxObj = Instantiate(boxPrefab, point.position, point.rotation);

        BoxScript box = boxObj.GetComponent<BoxScript>();
        if (box != null)
        {
            // Pick a random item from the available list
            if (availableItems.Count == 0)
            {
                Debug.LogWarning("No more unique items left. Using random from possibleItems.");
                box.SetRequiredItem(possibleItems[Random.Range(0, possibleItems.Length)]);
            }
            else
            {
                int index = Random.Range(0, availableItems.Count);
                box.SetRequiredItem(availableItems[index]);
                availableItems.RemoveAt(index); // ensures next box gets a different item
            }
        }
        else
        {
            Debug.LogWarning("Spawned box is missing BoxScript.");
        }
    }
}