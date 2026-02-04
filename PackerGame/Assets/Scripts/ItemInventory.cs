using UnityEngine;

public class ItemInventory : MonoBehaviour
{
    public int seedCount = 0;
    // taking seed, for seedChest
    public void Additem(int amount = 1)
    {
        seedCount += amount;
        Debug.Log("Seeds collected: " + seedCount);
    }
    public void PlaceItem(int amount = 1)
    {
        seedCount -= amount;
        Debug.Log("Seeds collected: " + seedCount);
    }
}
