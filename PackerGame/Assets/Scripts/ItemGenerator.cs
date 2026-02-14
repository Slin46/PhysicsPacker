using UnityEngine;
using System.Collections.Generic;

public class ItemGenerator : MonoBehaviour
{
    [Header("Box Spawning")]
    public GameObject boxPrefab;
    public Transform[] boxSpawnPoints;   // assign 5 in inspector

    [Header("Items")]
    public ItemType[] possibleItems;     // your item list

    void Start()
    {
        SpawnStartingBoxes();
    }

    void SpawnStartingBoxes()
    {
        if (boxPrefab == null || boxSpawnPoints.Length == 0 || possibleItems.Length == 0)
        {
            Debug.LogError("ItemGenerator not configured!");
            return;
        }

        // Create shuffled copy of items to ensure uniqueness
        List<ItemType> shuffledItems = new List<ItemType>(possibleItems);
        Shuffle(shuffledItems);

        int boxCount = Mathf.Min(5, boxSpawnPoints.Length, shuffledItems.Count);

        for (int i = 0; i < boxCount; i++)
        {
            GameObject boxObj = Instantiate(boxPrefab, boxSpawnPoints[i].position, Quaternion.identity);

            BoxScript box = boxObj.GetComponent<BoxScript>();
            if (box != null)
            {
                box.SetRequiredItem(shuffledItems[i]);
            }
        }
    }

    // Fisher-Yates shuffle
    void Shuffle(List<ItemType> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
    }
}
