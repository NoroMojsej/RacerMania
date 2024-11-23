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
    [Range (0,5)]public float sterrForce = 1;
    
    public float vertical;
    public float horizontal;
    public bool handbrake;
    
    public trackWaypoints waypoints;
    public List<Transform> nodes;
    public Transform currentWaypoint;
    
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
        vertical = .3f;
        AISteer();
    }
    
    private void userDrive()
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
        handbrake = (Input.GetAxis("Jump") != 0)? true : false;
    }
    
    private void calculateDistanceOfWaypoints() {
        Vector3 position = gameObject.transform.position;
        float distance = Mathf.Infinity;

        for (int i = 0; i < nodes.Count; i++) {
            Vector3 difference = nodes[i].transform.position - position;
            float currentDistance = difference.magnitude;
            if (currentDistance < distance) {
                
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
