using UnityEngine;

public enum ItemType
{
    Luigi,
    Book,
    Paper,
    Burger,
    Chair,
}

public class Item : MonoBehaviour
{
    public ItemType itemType; 
}