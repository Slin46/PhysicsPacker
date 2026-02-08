using UnityEngine;

public enum ItemType
{
    None,
    Luigi,
    Book,
    Paper,
}

public class Item : MonoBehaviour   // ← THIS is the important part
{
    public ItemType type;
}