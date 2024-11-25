using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Controller car;              // Reference to the player's car
    public GameObject carObject;
    public GameObject needle;           // Reference to the needle for the speedometer
    public TextMeshProUGUI timerText;   // Reference to the Timer UI TextMeshPro component
    public TextMeshProUGUI scoreText;
    private float startPosition = 223f, endPosition = -52.5f, desiredPosition;
    public float vehicleSpeed;

    private bool lapStarted = false;
    private bool lapEnded = false;

    private float raceTimer = 0f;       // Timer to track the race duration
    private int score = 0;              // Player's score
    
    public int countdownTime = 3; // Počiatočný čas na odpočítavanie (v sekundách)
    public TextMeshProUGUI countdownText;    // Odkaz na Text UI prvok

    public List<Controller> carControllers;
    private GameObject[] carObjects;
    public List<Vehicle> vehicles;
    public int currentPosition;
    public TextMeshProUGUI positionText;
    
    public List<GameObject> allCars;
    void Start()
    {
        SetCarLists();
        
        raceTimer = 0f;                 
        timerText.text = "Time: 0.00s"; 
        score = 0;                     // Initialize score
        scoreText.text = "Score: 0";   // Update UI


        StartCoroutine(StartCountdown());
        StartCoroutine(SortVehiclesTimedLoop());

    }

    private void SetCarLists()
    {
        carControllers = new List<Controller>();
        vehicles = new List<Vehicle>();
        carObject = GameObject.FindWithTag("Player");
        
        carControllers.Add(car);
        allCars = new List<GameObject>();
        allCars.Add(carObject);
        vehicles.Add(new Vehicle(carObject.GetComponent<InputManager>().passedWaypoints, car.name, car.hasFinished, true));
        
        // Nájde všetky GameObjecty s daným tagom
        carObjects = GameObject.FindGameObjectsWithTag("NPC");

        // Prejdi cez nájdené objekty
        foreach (GameObject obj in carObjects)
        {
            //Debug.Log("Found object: " + obj.name);
            carControllers.Add(obj.gameObject.GetComponent<Controller>());
            
            vehicles.Add(new Vehicle(obj.GetComponent<InputManager>().passedWaypoints, obj.gameObject.GetComponent<Controller>().name, 
                obj.gameObject.GetComponent<Controller>().hasFinished, false));
            
            allCars.Add(obj);
        }
        
    }

    private void SortVehicles()
    {
        for (int i = 0; i < allCars.Count; i++) {
            vehicles[i].hasFinished = allCars[i].GetComponent<Controller>().hasFinished;
            vehicles[i].name = allCars[i].GetComponent<Controller> ().name;
            vehicles[i].passedWaypoints = allCars[i].GetComponent<InputManager> ().passedWaypoints;
            //vehicles[i].isPlayer = allCars[i].GetComponent<InputManager>().
            if (allCars[i].GetComponent<InputManager>().driveController == InputManager.driver.User)
            {
                vehicles[i].isPlayer = true;
            }
            else
            {
                vehicles[i].isPlayer = false;
            }
        }
        
        for (int i = 0; i < vehicles.Count; i++) {
            for (int j = i + 1; j < vehicles.Count; j++) {
                if (vehicles[j].passedWaypoints < vehicles[i].passedWaypoints) {
                    Vehicle QQ = vehicles[i];
                    vehicles[i] = vehicles[j];
                    vehicles[j] = QQ;
                }
            }                
        }

        for (int i = 0; i < vehicles.Count; i++)
        {
            //Debug.Log(i +" " +vehicles[i].passedWaypoints + " " + vehicles[i].name + " " + vehicles[i].hasFinished + " " +vehicles[i].isPlayer);

            if (vehicles[i].isPlayer)
            {
                currentPosition = vehicles.Count-i;
                positionText.text = currentPosition + "/" + vehicles.Count; 
            }
        }

        //Debug.Log("---------------------------------------------------------------");
    }
    
    private IEnumerator SortVehiclesTimedLoop () {
        while (true) {
            yield return new WaitForSeconds (1);
            SortVehicles ();
        }
    }

    private void EnableControllers()
    {
        foreach (Controller con in carControllers)
        {
            con.isEnabled = true;
        }
    }

    // Method to add score
    public void AddScore(int value)
    {
        score += value;                 // Update the score
        scoreText.text = $"Score: {score}"; // Update score display
    }

    private void FixedUpdate()
    {
        vehicleSpeed = car.speed;
        updateNeedle();
    }

    void Update()
    {
        if (lapStarted && !lapEnded)
        {
            raceTimer += Time.deltaTime; // Increment the timer
            timerText.text = $"Time: {raceTimer:F2}s"; // Update timer display
        }
    }

    public void updateNeedle()
    {
        desiredPosition = startPosition - endPosition;
        float temp = vehicleSpeed / 100;
        needle.transform.eulerAngles = new Vector3(0, 0, (startPosition - temp * desiredPosition));
    }
    
    IEnumerator StartCountdown()
    {
        while (countdownTime > 0)
        {
            countdownText.text = countdownTime.ToString(); // Zobraz aktuálny čas
            //Debug.Log(countdownTime);
            yield return new WaitForSeconds(1);           // Počkaj 1 sekundu
            countdownTime--;                              // Zníž čas o 1
        }
        
        //Debug.Log("Go!");
        EnableControllers();
        StartCoroutine(GoTextCountdown());
    }
    
    IEnumerator GoTextCountdown()
    {
        countdownText.text = "GO!"; 
        yield return new WaitForSeconds(1); 
        countdownText.text = "";                       // Zobraz správu po odpočítaní
    }

    public void StartLap()
    {
        if (!lapStarted)
        {
            lapStarted = true;
            raceTimer = 0f;           // Reset timer when the race starts
            timerText.text = "Time: 0.00s"; // Reset UI text
        }
    }

    public void EndLap()
    {
        if (!lapEnded)
        {
            lapEnded = true;
            Debug.Log($"Race Ended! Total Time: {raceTimer:F2} seconds");
            timerText.text = $"Final Time: {raceTimer:F2}s";
        }
    }
}
