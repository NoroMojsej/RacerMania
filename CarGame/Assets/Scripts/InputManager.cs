using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public enum driver
    {
        AI,
        User
    }

    [SerializeField] public driver driveController;
    
    private int distanceOffset = 1;
    private float sterrForce = 2;
    
    public float vertical;
    public float horizontal;
    public bool handbrake;
    public bool reset;
    
    public trackWaypoints waypoints;
    public List<Transform> nodes;
    public Transform currentWaypoint;
    public Transform previousWaypoint;
    public int passedWaypoints = 0;
    
    
    private void Start()
    {
        waypoints = GameObject.FindGameObjectWithTag("Path").GetComponent<trackWaypoints>();
        currentWaypoint = gameObject.transform;
        nodes = waypoints.nodes;
    }
    
    

    private void FixedUpdate()
    {
        switch (driveController)
        {
            case driver.AI:
                AIDrive();
                break;
            case driver.User:
                userDrive();
                break;
        }
    }

    private void AIDrive()
    {
        calculateDistanceOfWaypoints();
        //vertical = .3f;
        vertical = .6f;
        AISteer();
    }
    
    private void userDrive()
    {
        calculateDistanceOfWaypoints();
        
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
        handbrake = (Input.GetAxis("Jump") != 0)? true : false;
        reset = Input.GetKey("r");
    }
    
    private void calculateDistanceOfWaypoints() {
        Vector3 position = gameObject.transform.position;
        float distance = Mathf.Infinity;

        for (int i = 0; i < nodes.Count; i++) {
            Vector3 difference = nodes[i].transform.position - position;
            float currentDistance = difference.magnitude;
            if (currentDistance < distance)
            {

                previousWaypoint = currentWaypoint;
                
                if (i + distanceOffset >= nodes.Count)
                {
                    currentWaypoint = nodes[0];
                }
                else
                {
                    currentWaypoint = nodes[i + distanceOffset];
                    
                    passedWaypoints = i + distanceOffset;
                }
                
                distance = currentDistance;
                

            }
        }
    }
    
    private void AISteer () {

        Vector3 relative = transform.InverseTransformPoint (currentWaypoint.transform.position);
        relative /= relative.magnitude;

        horizontal = (relative.x / relative.magnitude) * sterrForce;
    }
}
