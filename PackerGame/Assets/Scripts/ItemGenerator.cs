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

    private void Start()
    {
        SpawnAllItems();
    }

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
    public void SpawnBox()
    {}
}