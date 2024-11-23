using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpGreen : MonoBehaviour
{
    void Update()
    {
	    transform.Rotate(Vector3.up * Time.deltaTime * 10);
    }
    
    void OnTriggerEnter(Collider other) 
    {
    		
	    if(other.CompareTag("Player")) 
	    { 
		    Destroy(gameObject); 
		    PlayerPrefs.SetFloat("Money", PlayerPrefs.GetFloat("Money") + 100);
	    }
	}
}
