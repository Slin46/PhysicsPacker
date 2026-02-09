using UnityEngine;

public enum ItemType
{
    None,
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