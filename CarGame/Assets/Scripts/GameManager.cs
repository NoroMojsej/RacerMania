using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Controller car;           // Reference to the player's car
    public GameObject needle;        // Reference to the needle for the speedometer
    private float startPosition = 227f, endPosition = -52.5f, desiredPosition;
    public float vehicleSpeed;
    
    private bool raceStarted = false;
    private bool raceEnded = false;

    void Start()
    {
        // Initialize any race variables if needed
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        vehicleSpeed = car.speed;
        updateNeedle();
        
        if (raceStarted && !raceEnded)
        {
            // Additional logic for ongoing race, e.g., updating race timers or UI
        }
    }

    // Update the speedometer needle position
    public void updateNeedle()
    {
        desiredPosition = startPosition - endPosition;
        float temp = vehicleSpeed / 100;
        needle.transform.eulerAngles = new Vector3(0, 0, (startPosition - temp * desiredPosition));
    }

    // Start the race
    public void StartRace()
    {
        if (!raceStarted)
        {
            raceStarted = true;
            Debug.Log("Race Started!");
            // Additional start logic, like starting the timer or enabling UI elements
        }
    }

    // End the race
    public void EndRace()
    {
        if (!raceEnded)
        {
            raceEnded = true;
            Debug.Log("Race Ended!");
            // Additional end race logic, like showing results or stopping the timer
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, Screen.width/5, Screen.height/6), "MONEY: " + PlayerPrefs.GetFloat("Money"));
    }
}
