using UnityEngine;
using System.Collections.Generic;

public class ItemGenerator : MonoBehaviour
{
    public List<GameObject> itemPrefab = new List<GameObject>();
    public int randomList;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3 spawnPosition = new Vector3(-0.5f, 4f, 0f);
        Instantiate(itemPrefab[randomList], spawnPosition, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void RandomSpawnList()
    {
       randomList = Random.Range(0, itemPrefab.Count); 
       
    }
}
