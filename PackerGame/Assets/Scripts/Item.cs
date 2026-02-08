using UnityEngine;

public enum ItemType
{
    None,
    Luigi,
    Book,
    Paper,
}

public class Item : MonoBehaviour
{
    public ItemType itemType; 
}