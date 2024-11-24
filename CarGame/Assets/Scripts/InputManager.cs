using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    internal enum driver
    {
        AI,
        User
    }

    [SerializeField] private driver driveController;
    
    [Range (0,10)]public int distanceOffset = 1;
    [Range (0,5)]public float sterrForce = 2;
    
    public float vertical;
    public float horizontal;
    public bool handbrake;
    public bool reset;
    
    public trackWaypoints waypoints;
    public List<Transform> nodes;
    public Transform currentWaypoint;
    public Transform previousWaypoint;
    
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
                currentWaypoint = nodes[i + distanceOffset];
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
