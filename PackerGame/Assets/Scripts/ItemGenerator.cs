using UnityEngine;
using System.Collections.Generic;

public class ItemGenerator : MonoBehaviour
{
    public List<GameObject> itemPrefab = new List<GameObject>();
    public int randomList;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Spawn()
    {
       randomList = Random.Range(0, itemPrefab.Count); 
       Instantiate(itemPrefab[randomList], itemPrefab[randomList].GetComponent, Quaternion.identity);
    }
}
