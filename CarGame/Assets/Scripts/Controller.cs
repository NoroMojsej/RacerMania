using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Serialization;

public class Controller : MonoBehaviour
{
    private InputManager inputManager;
    public WheelCollider[] wheels = new WheelCollider[4];
    public GameObject[] wheelsMesh = new GameObject[4];
    public GameObject centerOfMass;
    private Rigidbody rigidbody;
    public float speed;
    public float maxSpeed = 80;
    public float brakePower = 100000000;
    private float radius = 6f;
    public float DownForceValue = 100;
    public float motorTorque = 1000;
    
 
    void Start()
    {
        getObjects();
    }

    private void FixedUpdate()
    {
        addDownForce();
        animateWheels();
        moveVehicle();
        steerVehicle();
    }
    

    private void moveVehicle()
    {
        if (speed < maxSpeed)
        {
            // Apply motor torque only if below max speed
            for (int i = 0; i < wheels.Length; i++)
            {
                wheels[i].motorTorque = inputManager.vertical * (motorTorque/4);
            }
        }
        else
        {
            // Prevent additional torque if max speed is reached
            for (int i = 0; i < wheels.Length; i++)
            {
                wheels[i].motorTorque = 0;
            }
        }
        
        speed = rigidbody.velocity.magnitude * 3.6f;

        if (inputManager.handbrake)
        {
            wheels[3].brakeTorque = wheels[2].brakeTorque = brakePower;
        }
        else
        {
            wheels[3].brakeTorque = wheels[2].brakeTorque = 0;
        }
    }

    private void steerVehicle()
    {
        //acerman steering formula
        //steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * horizontalInput;
         
        if (inputManager.horizontal > 0 ) 
        {
            //rear tracks size is set to 1.5f       wheel base has been set to 2.55f
            wheels[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * inputManager.horizontal;
            wheels[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius - (1.5f / 2))) * inputManager.horizontal;
        } 
        else if (inputManager.horizontal < 0 ) 
        {                                                          
            wheels[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius - (1.5f / 2))) * inputManager.horizontal;
            wheels[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * inputManager.horizontal;
            //transform.Rotate(Vector3.up * steerHelping);

        } 
        else 
        {
            wheels[0].steerAngle =0;
            wheels[1].steerAngle =0;
        }
    }

    void animateWheels()
    {
        Vector3 wheelPosition = Vector3.zero;
        Quaternion wheelRotation = Quaternion.identity;
        
        for (int i = 0; i < 4; i++)
        {
            wheels[i].GetWorldPose(out wheelPosition, out wheelRotation);
            wheelsMesh[i].transform.position = wheelPosition;
            wheelsMesh[i].transform.rotation = wheelRotation;
            
        } 
    }

    private void getObjects()
    {
        inputManager = GetComponent<InputManager>();
        rigidbody = GetComponent<Rigidbody>();
        centerOfMass = GameObject.Find("Mass");
        rigidbody.centerOfMass = centerOfMass.transform.localPosition;
    }

    private void addDownForce()
    {
        rigidbody.AddForce(-transform.up * DownForceValue * rigidbody.velocity.magnitude);
    }
}
