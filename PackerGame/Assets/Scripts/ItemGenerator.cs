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
    [Header("Item Spawn List")]
    public List<ItemSpawnData> items = new List<ItemSpawnData>();

    private void Start()
    {
        SpawnRandomItem();
    }

    public void SpawnRandomItem()
    {
        if (items.Count == 0)
        {
            Debug.LogWarning("No items assigned in ItemGenerator!");
            return;
        }

        int index = Random.Range(0, items.Count);

        ItemSpawnData data = items[index];

        if (data.itemPrefab == null || data.spawnPoint == null)
        {
            Debug.LogWarning("Item prefab or spawn point missing!");
            return;
        }

        Instantiate(data.itemPrefab, data.spawnPoint.position, data.spawnPoint.rotation);
    }
}