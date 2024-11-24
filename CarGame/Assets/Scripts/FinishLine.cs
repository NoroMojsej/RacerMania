using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private bool raceStarted = false;
    private bool raceEnded = false;

    // Reference to the GameManager
    public GameManager gameManager;

    void OnTriggerEnter(Collider other)
    {
        // Check if the player is crossing the finish line
        if (other.CompareTag("Player") || other.CompareTag("NPC"))
        {
            if (!raceStarted)
            {
                // Start the race on the first pass
                raceStarted = true;
                gameManager.StartRace();  // Calls StartRace() from GameManager
            }
            else if (raceStarted && !raceEnded)
            {
                // End the race on the second pass
                raceEnded = true;
                gameManager.EndRace();  // Calls EndRace() from GameManager
            }
        }
    }
}
