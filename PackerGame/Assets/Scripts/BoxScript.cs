using UnityEngine;
using UnityEngine.InputSystem;

public class BoxScript : MonoBehaviour
{
    public GameObject itemPrefab;
    public bool isPacked = false;
    public bool playerInside = false;
        private ItemInventory playerInventory; //ref
    // Update is called once per frame
   // void Update()
    //{
       // if (!playerInside || isPacked) return;
        // when pressed P key, call PlantSeed function
       // if (Keyboard.current.pKey.wasPressedThisFrame)
      //  {
            //if (playerInventory.PlaceItem()) //from seed inventory script
           // {
              //  PackBox();
          //  }
       // }
   // }  
    void PackBox()
    {
        isPacked = true;
        // notify quest manager
    }
}
