using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Controller car;              // Reference to the player's car
    public GameObject needle;           // Reference to the needle for the speedometer
    public TextMeshProUGUI timerText;   // Reference to the Timer UI TextMeshPro component
    public TextMeshProUGUI scoreText;
    private float startPosition = 227f, endPosition = -52.5f, desiredPosition;
    public float vehicleSpeed;

    private bool lapStarted = false;
    private bool lapEnded = false;

    private float raceTimer = 0f;       // Timer to track the race duration
    private int score = 0;              // Player's score
    
    public int countdownTime = 3; // Počiatočný čas na odpočítavanie (v sekundách)
    public TextMeshProUGUI countdownText;    // Odkaz na Text UI prvok

    public List<Controller> carControllers;
    private GameObject[] carObjects;
    void Start()
    {
        SetCarControllers();
        
        raceTimer = 0f;                 
        timerText.text = "Time: 0.00s"; 
        score = 0;                     // Initialize score
        scoreText.text = "Score: 0";   // Update UI


        StartCoroutine(StartCountdown());

    }

    private void SetCarControllers()
    {
        carControllers = new List<Controller>();
        carControllers.Add(car);
        
        // Nájde všetky GameObjecty s daným tagom
        carObjects = GameObject.FindGameObjectsWithTag("NPC");

        // Prejdi cez nájdené objekty
        foreach (GameObject obj in carObjects)
        {
            //Debug.Log("Found object: " + obj.name);
            carControllers.Add(obj.gameObject.GetComponent<Controller>());
        }
    }

    private void EnableControllers()
    {
        //Debug.Log("GO!");
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

        //countdownText.text = "GO!";                       // Zobraz správu po odpočítaní
        
        //OnCountdownFinished();                            // Voliteľná metóda po dokončení

        //Debug.Log("Go!");
        EnableControllers();
        StartCoroutine(GoTextCountdown());
        //countdownText.text = "";
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
