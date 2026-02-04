using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoxScript : MonoBehaviour
{
    public bool isInside = false;
    public bool isPacked = false;
    public Transform boxInteriorPoint; 
    private ItemGenerator item;
    // Update is called once per frame
   void Update()
    {
        
    }
    void OnTriggerEnter(Collider item)
    {
        // Check if the colliding object has the specific tag
        if (item.gameObject.CompareTag("Grabbable"))
        {

            // If it matches, execute specific actions here
            PackBox();
            Debug.Log("Pack!");
            // You can access other components or properties of the detected object using 'other.gameObject'
            // other.gameObject.GetComponent<SomeOtherScript>().DoSomething();
        }
    }
    void PackBox()
    {
        // notify quest manager
    }
        // A method to place an item inside the box
}
