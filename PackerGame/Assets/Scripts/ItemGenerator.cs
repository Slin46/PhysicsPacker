using UnityEngine;
using System.Collections.Generic;

public class ItemGenerator : MonoBehaviour
{
    public List<GameObject> itemPrefabs;
    public Transform spawnPoint;

    void Start()
    {
        SpawnRandomItem();
    }

    public void SpawnRandomItem()
    {
        int index = Random.Range(0, itemPrefabs.Count);
        Instantiate(itemPrefabs[index], spawnPoint.position, Quaternion.identity);
    }
}

