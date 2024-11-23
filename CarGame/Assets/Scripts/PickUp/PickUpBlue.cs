using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpBlue : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * 10);
    }
    
    void OnTriggerEnter(Collider other) 
    {
    		
        if(other.CompareTag("Player")) 
        { 
            other.GetComponent<Rigidbody>().AddForce(Vector3.forward * 100, ForceMode.Acceleration);
            Destroy(gameObject); 
        }
    }
}
