using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public int finishLineCrossCount = 2;

    // Reference to the GameManager
    public GameManager gameManager;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("NPC"))
        {
            Controller controller = other.GetComponent<Controller>();
            controller.finishLineCrossCount += 1;
            if (other.CompareTag("Player") && controller.finishLineCrossCount == 1)
            {
                Debug.Log("Race started for player.");
                gameManager.StartLap();
            }
            if (other.CompareTag("Player") && controller.finishLineCrossCount == finishLineCrossCount)
            {
                Debug.Log("Race ended for player.");
                controller.hasFinished = true;
                gameManager.EndLap();
            }
            else if (other.CompareTag("NPC") && controller.finishLineCrossCount == finishLineCrossCount)
            {
                Debug.Log("Race ended for npc.");
                controller.hasFinished = true;
            }

        }
    }
}
