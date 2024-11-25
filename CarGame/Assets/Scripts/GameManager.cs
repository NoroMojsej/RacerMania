using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Controller car;         
    public GameObject carObject;
    public GameObject needle;          
    public TextMeshProUGUI timerText; 
    public TextMeshProUGUI scoreText;
    private float startPosition = 223f, endPosition = -52.5f, desiredPosition;
    public float vehicleSpeed;

    private bool lapStarted = false;
    private bool lapEnded = false;

    private float raceTimer = 0f;  
    private int score = 0;    
    
    public int countdownTime = 3; // Počiatočný čas na odpočítavanie (v sekundách)
    public TextMeshProUGUI countdownText;    // Odkaz na Text UI prvok

    public List<Controller> carControllers;
    private GameObject[] carObjects;
    public List<Vehicle> vehicles;
    public int currentPosition;
    public TextMeshProUGUI positionText;
    
    public List<GameObject> allCars;

    private DatabaseReference databaseReference;

    void Start()
    {
        InitializeFirebase(); // Initializuje Firebase

        SetCarLists();
        
        raceTimer = 0f;                 
        timerText.text = "Time: 0.00s"; 
        score = 0;                     // Initialize score
        scoreText.text = "Score: 0";   // Update UI

        StartCoroutine(StartCountdown());
        StartCoroutine(SortVehiclesTimedLoop());
    }

    private void InitializeFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => 
        {
            if (task.Result == DependencyStatus.Available)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;
                databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
                Debug.Log("Firebase initialized successfully.");
            }
            else
            {
                Debug.LogError($"Could not resolve Firebase dependencies: {task.Result}");
            }
        });
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
            vehicles[i].name = allCars[i].GetComponent<Controller>().name;
            vehicles[i].passedWaypoints = allCars[i].GetComponent<InputManager>().passedWaypoints;
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
            if (vehicles[i].isPlayer)
            {
                currentPosition = vehicles.Count - i;
                positionText.text = currentPosition + "/" + vehicles.Count; 
            }
        }
    }
    
    private IEnumerator SortVehiclesTimedLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            SortVehicles();
        }
    }

    private void EnableControllers()
    {
        foreach (Controller con in carControllers)
        {
            con.isEnabled = true;
        }
    }

    public void AddScore(int value)
    {
        score += value;                 // updatne skóre
        scoreText.text = $"Score: {score}"; // updatne displej
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
            raceTimer += Time.deltaTime; // Incrementuje timer
            timerText.text = $"Time: {raceTimer:F2}s"; // Updatne displej
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
            yield return new WaitForSeconds(1);           // Počkaj 1 sekundu
            countdownTime--;                              // Zníž čas o 1
        }
        
        EnableControllers();
        StartCoroutine(GoTextCountdown());
    }
    
    IEnumerator GoTextCountdown()
    {
        countdownText.text = "GO!"; 
        yield return new WaitForSeconds(1); 
        countdownText.text = "";                       
    }

    public void StartLap()
    {
        if (!lapStarted)
        {
            lapStarted = true;
            raceTimer = 0f;           
            timerText.text = "Time: 0.00s";
        }
    }

    public void EndLap()
    {
        if (!lapEnded)
        {
            lapEnded = true;
            timerText.text = $"Final Time: {raceTimer:F2}s";

            // Uloží na firebase
            SaveRaceData(SceneManager.GetActiveScene().name, score, raceTimer);
        }
    }

    private void SaveRaceData(string level, int score, float time)
    {
        if (databaseReference == null)
        {
            Debug.LogError("Database reference not initialized.");
            return;
        }

        string userId = System.Guid.NewGuid().ToString(); 

        Dictionary<string, object> raceData = new Dictionary<string, object>
        {
            { "level", level },
            { "score", score },
            { "time", time },
            { "date", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }
        };

        databaseReference.Child("races").Child(userId).SetValueAsync(raceData).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Race data saved successfully!");
            }
            else
            {
                Debug.LogError($"Failed to save race data: {task.Exception}");
            }
        });
    }
}
