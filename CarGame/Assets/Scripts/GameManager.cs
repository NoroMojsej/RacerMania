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

    private bool raceStarted = false;
    private bool raceEnded = false;

    private float raceTimer = 0f;       // Timer to track the race duration
    private int score = 0;             // Player's score

    void Start()
    {
        raceTimer = 0f;                 
        timerText.text = "Time: 0.00s"; 
        score = 0;                     // Initialize score
        scoreText.text = "Score: 0";   // Update UI
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
        if (raceStarted && !raceEnded)
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

    public void StartRace()
    {
        if (!raceStarted)
        {
            raceStarted = true;
            raceTimer = 0f;           // Reset timer when the race starts
            timerText.text = "Time: 0.00s"; // Reset UI text
        }
    }

    public void EndRace()
    {
        if (!raceEnded)
        {
            raceEnded = true;
            Debug.Log($"Race Ended! Total Time: {raceTimer:F2} seconds");
            timerText.text = $"Final Time: {raceTimer:F2}s";
        }
    }
}
