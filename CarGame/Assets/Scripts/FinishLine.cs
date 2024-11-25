using UnityEngine;

public class FinishLine : MonoBehaviour
{
    //private bool raceStarted = false;
    //private bool raceEnded = false;

    public int finishLineCrossCount = 2;

    // Reference to the GameManager
    public GameManager gameManager;

    void OnTriggerEnter(Collider other)
    {
        // Check if the player or NPC is crossing the finish line
        /*if (other.CompareTag("Player") || other.CompareTag("NPC"))
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
        }*/
        if (other.CompareTag("Player") || other.CompareTag("NPC"))
        {
            other.GetComponent<Controller>().finishLineCrossCount += 1;
            if (other.CompareTag("Player") && other.GetComponent<Controller>().finishLineCrossCount == 1)
            {
                Debug.Log("Race started for player.");
                gameManager.StartLap();
            }
            if (other.CompareTag("Player") && other.GetComponent<Controller>().finishLineCrossCount == finishLineCrossCount)
            {
                Debug.Log("Race ended for player.");
                gameManager.EndLap();
            }
        }
    }
}
