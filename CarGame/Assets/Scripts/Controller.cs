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
    public float brakePower = 3000;
    //private float radius = 6f;
    public float DownForceValue = 1000;
    public float motorTorque = 1000;
    public float steeringMax = 5;

    // How many times car crossed finnish line
    public int finishLineCrossCount = 0;
    public bool hasFinished = false;
    public bool isEnabled = false;
    public string name;
    
 
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
        adjustFriction();
    }
    
    

    private void moveVehicle()
    {
        if (isEnabled)
        {
            if (speed < maxSpeed)
            {
                // Apply motor torque only to the rear wheels for better control
                wheels[2].motorTorque = inputManager.vertical * motorTorque / 2; // Rear-right
                wheels[3].motorTorque = inputManager.vertical * motorTorque / 2; // Rear-left
            }
            else
            {
                // Prevent additional torque if max speed is reached
                foreach (WheelCollider wheel in wheels)
                {
                    wheel.motorTorque = 0;
                }
            }
        
        speed = rigidbody.velocity.magnitude * 3.6f;
        
        if (inputManager.handbrake)
        {
            foreach (WheelCollider wheel in wheels)
            {
                wheel.brakeTorque = brakePower;
            }
            
            foreach (WheelCollider wheel in wheels)
            {
                WheelFrictionCurve friction = wheel.forwardFriction;
                friction.stiffness = 2f; // Lower value for more slide
                wheel.forwardFriction = friction;

                friction = wheel.sidewaysFriction;
                friction.stiffness = 2f; // Lower value for drift-like side sliding
                wheel.sidewaysFriction = friction;
            }
        }
        else
        {
            foreach (WheelCollider wheel in wheels)
            {
                wheel.brakeTorque = 0;
            }
            
            foreach (WheelCollider wheel in wheels)
            {
                wheel.brakeTorque = 0;

                WheelFrictionCurve friction = wheel.forwardFriction;
                friction.stiffness = 1.0f; // Default stiffness
                wheel.forwardFriction = friction;

                friction = wheel.sidewaysFriction;
                friction.stiffness = 1.0f; // Default stiffness
                wheel.sidewaysFriction = friction;
            }
        }
        
        if (inputManager.reset)
            {
                transform.position = inputManager.previousWaypoint.position;

                rigidbody.velocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;
            } 
        }
        
    }

    private void steerVehicle()
    {
        //acerman steering formula
        //steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * horizontalInput;
         
        
        /*if (inputManager.horizontal > 0 ) 
        {
            //rear tracks size is set to 1.5f       wheel base has been set to 2.55f
            /*wheels[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * inputManager.horizontal;
            wheels[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius - (1.5f / 2))) * inputManager.horizontal;#1#
            
            //rear track size 5f wheel base 10f
            wheels[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(10f / (radius + (5f / 2))) * inputManager.horizontal;
            wheels[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(10f / (radius - (5f / 2))) * inputManager.horizontal;
        } 
        else if (inputManager.horizontal < 0 ) 
        {                                                          
            /*wheels[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius - (1.5f / 2))) * inputManager.horizontal;
            wheels[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * inputManager.horizontal;#1#
            //transform.Rotate(Vector3.up * steerHelping);
            
            wheels[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(10f / (radius - (5f / 2))) * inputManager.horizontal;
            wheels[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(10f / (radius + (5f / 2))) * inputManager.horizontal;

        }
        else 
        {
            wheels[0].steerAngle =0;
            wheels[1].steerAngle =0;
        }*/

        if (inputManager.horizontal != 0)
        {
            wheels[0].steerAngle = inputManager.horizontal * steeringMax;
            wheels[1].steerAngle = inputManager.horizontal * steeringMax;
        }
        else
        {
            wheels[0].steerAngle = 0;
            wheels[1].steerAngle = 0;
        }
    }
    
    private void adjustFriction()
    {
        foreach (WheelCollider wheel in wheels)
        {
            /*WheelFrictionCurve forwardFriction = wheel.forwardFriction;
            WheelFrictionCurve sidewaysFriction = wheel.sidewaysFriction;

            // Increase grip at higher speeds
            float speedFactor = Mathf.Clamp01(rigidbody.velocity.magnitude / 50f); // 50 is an example threshold
            forwardFriction.stiffness = Mathf.Lerp(1.0f, 2.0f, speedFactor); // Higher stiffness at high speed
            sidewaysFriction.stiffness = Mathf.Lerp(1.0f, 2.5f, speedFactor);

            wheel.forwardFriction = forwardFriction;
            wheel.sidewaysFriction = sidewaysFriction;*/
            
            WheelFrictionCurve forwardFriction = wheel.forwardFriction;
            WheelFrictionCurve sidewaysFriction = wheel.sidewaysFriction;
            
            forwardFriction.stiffness = 2.5f; // High stiffness for strong grip
            sidewaysFriction.stiffness = 3.0f; // Prevent lateral sliding
            
            wheel.forwardFriction = forwardFriction;
            wheel.sidewaysFriction = sidewaysFriction;
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
        //centerOfMass = GameObject.Find("Mass");
        rigidbody.centerOfMass = centerOfMass.transform.localPosition;
    }

    private void addDownForce()
    {
        //rigidbody.AddForce(-transform.up * DownForceValue * rigidbody.velocity.magnitude);
        //rigidbody.AddForce(-transform.up * DownForceValue * Mathf.Clamp(rigidbody.velocity.magnitude, 0, 50));
        
        float speedFactor = Mathf.Clamp(rigidbody.velocity.magnitude, 0, maxSpeed); // Cap the speed factor
        rigidbody.AddForce(-transform.up * DownForceValue * speedFactor); // Scale downforce with speed
    }
}
