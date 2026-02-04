using UnityEngine;

public class Balance : MonoBehaviour
{
    public Transform body;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = body.position;
    }
}
